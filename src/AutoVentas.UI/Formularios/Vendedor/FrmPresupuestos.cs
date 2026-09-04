using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Vendedor;

/// <summary>Gestión de Presupuestos (Vendedor). Un presupuesto aprobado puede dar origen a un Contrato.</summary>
public partial class FrmPresupuestos : Form, IObservadorIdioma
{
    private readonly GestorPresupuestos _gestor = new();
    private readonly ControladorListadoCrud<Presupuesto> _controlador;

    public FrmPresupuestos()
    {
        InitializeComponent();

        _controlador = new ControladorListadoCrud<Presupuesto>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, p => _gestor.Eliminar(p.IdPresupuesto));

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
        using var frm = new FrmPresupuestoEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Presupuesto seleccionado)
    {
        using var frm = new FrmPresupuestoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.presupuestos");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
