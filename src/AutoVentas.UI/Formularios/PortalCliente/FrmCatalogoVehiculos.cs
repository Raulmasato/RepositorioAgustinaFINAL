using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.PortalCliente;

/// <summary>Catálogo de vehículos disponibles, de solo lectura, visible para el rol Cliente.
/// Desde acá el cliente puede iniciar una reserva sobre el vehículo seleccionado.</summary>
public partial class FrmCatalogoVehiculos : Form, IObservadorIdioma
{
    private readonly IGestorVehiculos _gestorVehiculos = new GestorVehiculos();
    private readonly IGestorClientes _gestorClientes = new GestorClientes();
    private List<Vehiculo> _vehiculosDisponibles = new();

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
        _vehiculosDisponibles = _gestorVehiculos.ObtenerDisponibles();
        AplicarFiltro();
    }

    /// <summary>Filtra en memoria (la lista de disponibles ya está completa en
    /// <see cref="_vehiculosDisponibles"/>) por marca o modelo, sin distinguir mayúsculas de
    /// minúsculas, a medida que el cliente escribe en el cuadro de búsqueda.</summary>
    private void AplicarFiltro()
    {
        var texto = _txtBuscar.Text.Trim();
        var filtrados = texto.Length == 0
            ? _vehiculosDisponibles
            : _vehiculosDisponibles.Where(v =>
                v.Marca.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                v.Modelo.Contains(texto, StringComparison.OrdinalIgnoreCase)).ToList();

        _grilla.DataSource = null;
        _grilla.DataSource = filtrados;
    }

    private void TxtBuscar_TextChanged(object? sender, EventArgs e) => AplicarFiltro();

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
        _lblBuscar.Text = t.Traducir("btn.buscar");
        _btnReservar.Text = t.Traducir("btn.reservar");
    }
}
