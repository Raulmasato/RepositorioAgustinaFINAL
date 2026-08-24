using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.PortalCliente;

/// <summary>
/// "Crear reserva" desde la óptica del Cliente: solo puede crear y listar sus propias
/// reservas (a diferencia del Ejecutivo, que tiene el CRUD completo sobre todas las reservas).
/// </summary>
public partial class FrmMisReservas : Form, IObservadorIdioma
{
    private readonly GestorReservas _gestorReservas = new();
    private readonly GestorClientes _gestorClientes = new();

    public FrmMisReservas()
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

    private Domain.Entidades.Cliente? ObtenerClienteActual()
    {
        var usuario = SesionActual.Instancia.UsuarioLogueado!;
        return _gestorClientes.ObtenerPorUsuario(usuario.IdUsuario);
    }

    private void Refrescar()
    {
        var cliente = ObtenerClienteActual();
        _grilla.DataSource = null;
        _grilla.DataSource = cliente is null ? new List<Reserva>() : _gestorReservas.ObtenerPorCliente(cliente.IdCliente);
    }

    private void BtnNuevaReserva_Click(object? sender, EventArgs e)
    {
        var cliente = ObtenerClienteActual();
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

    private void BtnRefrescar_Click(object? sender, EventArgs e)
    {
        Refrescar();
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.reservas");
        _btnNuevaReserva.Text = t.Traducir("btn.nuevareserva");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
