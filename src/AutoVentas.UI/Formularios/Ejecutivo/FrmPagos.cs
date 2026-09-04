using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Pagos (Ejecutivo).</summary>
public partial class FrmPagos : Form, IObservadorIdioma
{
    private readonly GestorPagos _gestor = new();
    private readonly ControladorListadoCrud<Pago> _controlador;

    public FrmPagos()
    {
        InitializeComponent();

        _controlador = new ControladorListadoCrud<Pago>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, p => _gestor.Eliminar(p.IdPago));

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
        using var frm = new FrmPagoEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Pago seleccionado)
    {
        using var frm = new FrmPagoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.pagos");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
