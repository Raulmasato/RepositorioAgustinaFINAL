using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Editor de reserva. Cuando <paramref name="clienteFijo"/> se especifica, la reserva
/// queda acotada a ese cliente (usado por el menú de Cliente, que solo puede reservar para sí mismo).</summary>
internal partial class FrmReservaEditar : Form, IObservadorIdioma
{
    private readonly GestorReservas _gestor = new();
    private readonly Reserva? _original;
    private readonly Cliente? _clienteFijo;

    public FrmReservaEditar(Reserva? reserva, Cliente? clienteFijo = null)
    {
        _original = reserva;
        _clienteFijo = clienteFijo;
        InitializeComponent();
        _dtpVencimiento.Value = DateTime.Now.AddDays(7);

        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoReserva>().Cast<object>().ToArray());

        if (clienteFijo is not null)
        {
            _cmbCliente.Items.Add(clienteFijo);
            _cmbCliente.SelectedItem = clienteFijo;
            _cmbCliente.Enabled = false;
            _cmbEstado.Enabled = false; // el cliente crea la reserva como Pendiente, la confirma el ejecutivo
        }

        if (reserva is not null)
        {
            _dtpVencimiento.Value = reserva.FechaVencimiento;
            _cmbEstado.SelectedItem = reserva.Estado;
        }
        else
        {
            _cmbEstado.SelectedItem = EstadoReserva.Pendiente;
        }

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);

        // Diferido a Load: el diseñador de Visual Studio no debe ejecutar consultas a la BD
        // al instanciar este formulario para dibujarlo.
        Load += (_, _) =>
        {
            CargarCombosDependientesDeBD();
            ActualizarIdioma();
        };
    }

    private void CargarCombosDependientesDeBD()
    {
        _cmbVehiculo.Items.AddRange(new GestorVehiculos().ObtenerDisponibles().Cast<object>().ToArray());

        if (_clienteFijo is null)
        {
            _cmbCliente.Items.AddRange(new GestorClientes().ObtenerTodos().Cast<object>().ToArray());
        }

        if (_original is not null)
        {
            _cmbVehiculo.Items.Clear();
            _cmbVehiculo.Items.AddRange(new GestorVehiculos().ObtenerTodos().Cast<object>().ToArray());
            _cmbVehiculo.SelectedItem = _cmbVehiculo.Items.Cast<Vehiculo>().FirstOrDefault(v => v.IdVehiculo == _original.IdVehiculo);
            if (_clienteFijo is null)
            {
                _cmbCliente.SelectedItem = _cmbCliente.Items.Cast<Cliente>().FirstOrDefault(c => c.IdCliente == _original.IdCliente);
            }
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbVehiculo.SelectedItem is not Vehiculo vehiculo || _cmbCliente.SelectedItem is not Cliente cliente)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.seleccionevehiculocliente"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var usuario = SesionActual.Instancia.UsuarioLogueado!;
            var reserva = _original ?? new Reserva
            {
                FechaReserva = DateTime.Now,
                IdUsuarioEjecutivo = _clienteFijo is null ? usuario.IdUsuario : null
            };
            reserva.IdVehiculo = vehiculo.IdVehiculo;
            reserva.IdCliente = cliente.IdCliente;
            reserva.FechaVencimiento = _dtpVencimiento.Value;
            reserva.Estado = (EstadoReserva)_cmbEstado.SelectedItem!;

            if (_original is null) _gestor.Agregar(reserva);
            else _gestor.Modificar(reserva);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.reservas");
        _lblVehiculo.Text = t.Traducir("lbl.vehiculo");
        _lblCliente.Text = t.Traducir("lbl.cliente");
        _lblVencimiento.Text = t.Traducir("lbl.vencimiento");
        _lblEstado.Text = t.Traducir("lbl.estado");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
