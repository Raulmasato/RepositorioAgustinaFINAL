using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Reservas — CRUD completo a cargo del Ejecutivo.</summary>
public class FrmReservas : FormListadoBase<Reserva>
{
    private readonly GestorReservas _gestor = new();

    protected override string ClaveTituloIdioma => "menu.reservas";

    protected override void ConfigurarColumnas(DataGridView g)
    {
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.IdReserva), HeaderText = "Id", Width = 50 });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.VehiculoDescripcion), HeaderText = "Vehículo" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.ClienteNombreCompleto), HeaderText = "Cliente" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.FechaReserva), HeaderText = "Fecha reserva" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.FechaVencimiento), HeaderText = "Vence" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.Estado), HeaderText = "Estado" });
    }

    protected override List<Reserva> ObtenerDatos() => _gestor.ObtenerTodos();

    protected override void AbrirAlta()
    {
        using var frm = new FrmReservaEditar(null);
        frm.ShowDialog(this);
    }

    protected override void AbrirEdicion(Reserva seleccionado)
    {
        using var frm = new FrmReservaEditar(seleccionado);
        frm.ShowDialog(this);
    }

    protected override void Eliminar(Reserva seleccionado) => _gestor.Eliminar(seleccionado.IdReserva);
}

/// <summary>Editor de reserva. Cuando <paramref name="clienteFijo"/> se especifica, la reserva
/// queda acotada a ese cliente (usado por el menú de Cliente, que solo puede reservar para sí mismo).</summary>
internal class FrmReservaEditar : Form, IObservadorIdioma
{
    private readonly GestorReservas _gestor = new();
    private readonly Reserva? _original;
    private readonly Cliente? _clienteFijo;

    private readonly Label _lblVehiculo = new() { Left = 20, Top = 20, Width = 100 };
    private readonly ComboBox _cmbVehiculo = new() { Left = 130, Top = 17, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblCliente = new() { Left = 20, Top = 55, Width = 100 };
    private readonly ComboBox _cmbCliente = new() { Left = 130, Top = 52, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblVencimiento = new() { Left = 20, Top = 90, Width = 100 };
    private readonly DateTimePicker _dtpVencimiento = new() { Left = 130, Top = 87, Width = 220 };
    private readonly Label _lblEstado = new() { Left = 20, Top = 125, Width = 100 };
    private readonly ComboBox _cmbEstado = new() { Left = 130, Top = 122, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 165, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 165, Width = 90 };

    public FrmReservaEditar(Reserva? reserva, Cliente? clienteFijo = null)
    {
        _original = reserva;
        _clienteFijo = clienteFijo;
        Width = 400;
        Height = 250;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        _dtpVencimiento.Value = DateTime.Now.AddDays(7);

        Controls.AddRange(new Control[]
        {
            _lblVehiculo, _cmbVehiculo, _lblCliente, _cmbCliente, _lblVencimiento, _dtpVencimiento,
            _lblEstado, _cmbEstado, _btnGuardar, _btnCancelar
        });

        _cmbVehiculo.Items.AddRange(new GestorVehiculos().ObtenerDisponibles().Cast<object>().ToArray());
        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoReserva>().Cast<object>().ToArray());

        if (clienteFijo is not null)
        {
            _cmbCliente.Items.Add(clienteFijo);
            _cmbCliente.SelectedItem = clienteFijo;
            _cmbCliente.Enabled = false;
            _cmbEstado.Enabled = false; // el cliente crea la reserva como Pendiente, la confirma el ejecutivo
        }
        else
        {
            _cmbCliente.Items.AddRange(new GestorClientes().ObtenerTodos().Cast<object>().ToArray());
        }

        if (reserva is not null)
        {
            _cmbVehiculo.Items.Clear();
            _cmbVehiculo.Items.AddRange(new GestorVehiculos().ObtenerTodos().Cast<object>().ToArray());
            _cmbVehiculo.SelectedItem = _cmbVehiculo.Items.Cast<Vehiculo>().FirstOrDefault(v => v.IdVehiculo == reserva.IdVehiculo);
            if (clienteFijo is null)
            {
                _cmbCliente.SelectedItem = _cmbCliente.Items.Cast<Cliente>().FirstOrDefault(c => c.IdCliente == reserva.IdCliente);
            }
            _dtpVencimiento.Value = reserva.FechaVencimiento;
            _cmbEstado.SelectedItem = reserva.Estado;
        }
        else
        {
            _cmbEstado.SelectedItem = EstadoReserva.Pendiente;
        }

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
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
