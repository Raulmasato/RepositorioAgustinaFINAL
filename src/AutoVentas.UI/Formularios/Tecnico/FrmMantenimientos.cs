using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Tecnico;

/// <summary>Gestión de Mantenimientos (Técnico).</summary>
public class FrmMantenimientos : Form, IObservadorIdioma
{
    private readonly GestorMantenimientos _gestor = new();

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
    private readonly ControladorListadoCrud<Mantenimiento> _controlador;

    public FrmMantenimientos()
    {
        Width = 820;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.IdMantenimiento), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.VehiculoDescripcion), HeaderText = "Vehículo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.ClienteNombreCompleto), HeaderText = "Cliente" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.Servicio), HeaderText = "Servicio" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.FechaServicio), HeaderText = "Fecha" });

        _panelBotones.Controls.AddRange(new Control[] { _btnNuevo, _btnEditar, _btnEliminar, _btnRefrescar });
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _controlador = new ControladorListadoCrud<Mantenimiento>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, m => _gestor.Eliminar(m.IdMantenimiento));

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

internal class FrmMantenimientoEditar : Form, IObservadorIdioma
{
    private readonly GestorMantenimientos _gestor = new();
    private readonly Mantenimiento? _original;

    private readonly Label _lblVehiculo = new() { Left = 20, Top = 20, Width = 100 };
    private readonly ComboBox _cmbVehiculo = new() { Left = 130, Top = 17, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblCliente = new() { Left = 20, Top = 55, Width = 100 };
    private readonly ComboBox _cmbCliente = new() { Left = 130, Top = 52, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblServicio = new() { Left = 20, Top = 90, Width = 100 };
    private readonly TextBox _txtServicio = new() { Left = 130, Top = 87, Width = 220 };
    private readonly Label _lblFecha = new() { Left = 20, Top = 125, Width = 100 };
    private readonly DateTimePicker _dtpFecha = new() { Left = 130, Top = 122, Width = 220 };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 165, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 165, Width = 90 };

    public FrmMantenimientoEditar(Mantenimiento? mantenimiento)
    {
        _original = mantenimiento;
        Width = 400;
        Height = 250;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[]
        {
            _lblVehiculo, _cmbVehiculo, _lblCliente, _cmbCliente, _lblServicio, _txtServicio,
            _lblFecha, _dtpFecha, _btnGuardar, _btnCancelar
        });

        if (mantenimiento is not null)
        {
            _txtServicio.Text = mantenimiento.Servicio;
            _dtpFecha.Value = mantenimiento.FechaServicio;
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
        _cmbVehiculo.Items.AddRange(new GestorVehiculos().ObtenerTodos().Cast<object>().ToArray());
        _cmbCliente.Items.AddRange(new GestorClientes().ObtenerTodos().Cast<object>().ToArray());

        if (_original is not null)
        {
            _cmbVehiculo.SelectedItem = _cmbVehiculo.Items.Cast<Vehiculo>().FirstOrDefault(v => v.IdVehiculo == _original.IdVehiculo);
            _cmbCliente.SelectedItem = _cmbCliente.Items.Cast<Cliente>().FirstOrDefault(c => c.IdCliente == _original.IdCliente);
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
            var mantenimiento = _original ?? new Mantenimiento();
            mantenimiento.IdVehiculo = vehiculo.IdVehiculo;
            mantenimiento.IdCliente = cliente.IdCliente;
            mantenimiento.Servicio = _txtServicio.Text.Trim();
            mantenimiento.FechaServicio = _dtpFecha.Value;

            if (_original is null) _gestor.Agregar(mantenimiento);
            else _gestor.Modificar(mantenimiento);

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
        Text = t.Traducir("menu.mantenimientos");
        _lblVehiculo.Text = t.Traducir("lbl.vehiculo");
        _lblCliente.Text = t.Traducir("lbl.cliente");
        _lblServicio.Text = t.Traducir("lbl.servicio");
        _lblFecha.Text = t.Traducir("lbl.fecha");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
