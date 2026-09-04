using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Vendedor;

/// <summary>Gestión de Clientes (a cargo del Vendedor).</summary>
public partial class FrmClientes : Form, IObservadorIdioma
{
    private readonly GestorClientes _gestor = new();
    private readonly ControladorListadoCrud<Cliente> _controlador;

    public FrmClientes()
    {
        InitializeComponent();

        _controlador = new ControladorListadoCrud<Cliente>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, c => _gestor.Eliminar(c.IdCliente));

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
        using var frm = new FrmClienteEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Cliente seleccionado)
    {
        using var frm = new FrmClienteEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.clientes");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
