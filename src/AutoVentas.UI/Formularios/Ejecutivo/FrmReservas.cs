using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Reservas — CRUD completo a cargo del Ejecutivo.</summary>
public partial class FrmReservas : Form, IObservadorIdioma
{
    private readonly GestorReservas _gestor = new();
    private readonly ControladorListadoCrud<Reserva> _controlador;

    public FrmReservas()
    {
        InitializeComponent();

        _controlador = new ControladorListadoCrud<Reserva>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, r => _gestor.Eliminar(r.IdReserva));

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            _controlador.Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void BtnNuevo_Click(object? sender, EventArgs e) => _controlador.Nuevo();

    private void BtnEditar_Click(object? sender, EventArgs e) => _controlador.Editar();

    private void BtnEliminar_Click(object? sender, EventArgs e) => _controlador.EliminarSeleccionado();

    private void BtnRefrescar_Click(object? sender, EventArgs e) => _controlador.Refrescar();

    private void AbrirAlta()
    {
        using var frm = new FrmReservaEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Reserva seleccionado)
    {
        using var frm = new FrmReservaEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.reservas");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
