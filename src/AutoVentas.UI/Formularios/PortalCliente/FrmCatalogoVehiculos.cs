using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.PortalCliente;

/// <summary>Catálogo de vehículos disponibles, de solo lectura, visible para el rol Cliente.
/// Desde acá el cliente puede iniciar una reserva sobre el vehículo seleccionado.</summary>
public partial class FrmCatalogoVehiculos : Form, IObservadorIdioma
{
    private readonly GestorVehiculos _gestorVehiculos = new();
    private readonly GestorClientes _gestorClientes = new();

    public FrmCatalogoVehiculos()
    {
        InitializeComponent();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void Refrescar()
    {
        _grilla.DataSource = null;
        _grilla.DataSource = _gestorVehiculos.ObtenerDisponibles();
    }

    private void BtnReservar_Click(object? sender, EventArgs e)
    {
        if (_grilla.SelectedRows.Count == 0) return;

        var usuario = SesionActual.Instancia.UsuarioLogueado!;
        var cliente = _gestorClientes.ObtenerPorUsuario(usuario.IdUsuario);
        if (cliente is null)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.clientenoencontrado"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var frm = new Ejecutivo.FrmReservaEditar(null, cliente);
        if (frm.ShowDialog(this) == DialogResult.OK)
        {
            Refrescar();
        }
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.vehiculos");
        _btnReservar.Text = t.Traducir("btn.reservar");
    }
}
