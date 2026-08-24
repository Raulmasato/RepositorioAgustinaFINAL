using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Tecnico;

/// <summary>Gestión de Mantenimientos (Técnico).</summary>
public partial class FrmMantenimientos : Form, IObservadorIdioma
{
    private readonly GestorMantenimientos _gestor = new();
    private readonly ControladorListadoCrud<Mantenimiento> _controlador;

    public FrmMantenimientos()
    {
        InitializeComponent();

        _controlador = new ControladorListadoCrud<Mantenimiento>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, m => _gestor.Eliminar(m.IdMantenimiento));

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
        using var frm = new FrmMantenimientoEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Mantenimiento seleccionado)
    {
        using var frm = new FrmMantenimientoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.mantenimientos");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
