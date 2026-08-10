using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Reservas — CRUD completo a cargo del Ejecutivo.</summary>
public class FrmReservas : Form, IObservadorIdioma
{
    private readonly GestorReservas _gestor = new();

    private readonly DataGridView _grilla = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoGenerateColumns = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly FlowLayoutPanel _panelBotones = new() { Dock = DockStyle.Top, Height = 42, Padding = new Padding(6) };
    private readonly Button _btnNuevo = new() { AutoSize = true };
    private readonly Button _btnEditar = new() { AutoSize = true };
    private readonly Button _btnEliminar = new() { AutoSize = true };
    private readonly Button _btnRefrescar = new() { AutoSize = true };
    private readonly ControladorListadoCrud<Reserva> _controlador;

    public FrmReservas()
    {
        Width = 820;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.IdReserva), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.VehiculoDescripcion), HeaderText = "Vehículo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.ClienteNombreCompleto), HeaderText = "Cliente" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.FechaReserva), HeaderText = "Fecha reserva" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.FechaVencimiento), HeaderText = "Vence" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.Estado), HeaderText = "Estado" });

        _panelBotones.Controls.AddRange(new Control[] { _btnNuevo, _btnEditar, _btnEliminar, _btnRefrescar });
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _controlador = new ControladorListadoCrud<Reserva>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, r => _gestor.Eliminar(r.IdReserva));

        _btnNuevo.Click += (_, _) => _controlador.Nuevo();
        _btnEditar.Click += (_, _) => _controlador.Editar();
        _btnEliminar.Click += (_, _) => _controlador.EliminarSeleccionado();
        _btnRefrescar.Click += (_, _) => _controlador.Refrescar();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            _controlador.Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

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

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

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
