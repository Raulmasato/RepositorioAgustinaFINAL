using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Tecnico;

/// <summary>Gestión de Mantenimientos (Técnico).</summary>
public class FrmMantenimientos : FormListadoBase<Mantenimiento>
{
    private readonly GestorMantenimientos _gestor = new();

    protected override string ClaveTituloIdioma => "menu.mantenimientos";

    protected override void ConfigurarColumnas(DataGridView g)
    {
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.IdMantenimiento), HeaderText = "Id", Width = 50 });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.VehiculoDescripcion), HeaderText = "Vehículo" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.ClienteNombreCompleto), HeaderText = "Cliente" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.Servicio), HeaderText = "Servicio" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Mantenimiento.FechaServicio), HeaderText = "Fecha" });
    }

    protected override List<Mantenimiento> ObtenerDatos() => _gestor.ObtenerTodos();

    protected override void AbrirAlta()
    {
        using var frm = new FrmMantenimientoEditar(null);
        frm.ShowDialog(this);
    }

    protected override void AbrirEdicion(Mantenimiento seleccionado)
    {
        using var frm = new FrmMantenimientoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    protected override void Eliminar(Mantenimiento seleccionado) => _gestor.Eliminar(seleccionado.IdMantenimiento);
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

        _cmbVehiculo.Items.AddRange(new GestorVehiculos().ObtenerTodos().Cast<object>().ToArray());
        _cmbCliente.Items.AddRange(new GestorClientes().ObtenerTodos().Cast<object>().ToArray());

        if (mantenimiento is not null)
        {
            _cmbVehiculo.SelectedItem = _cmbVehiculo.Items.Cast<Vehiculo>().FirstOrDefault(v => v.IdVehiculo == mantenimiento.IdVehiculo);
            _cmbCliente.SelectedItem = _cmbCliente.Items.Cast<Cliente>().FirstOrDefault(c => c.IdCliente == mantenimiento.IdCliente);
            _txtServicio.Text = mantenimiento.Servicio;
            _dtpFecha.Value = mantenimiento.FechaServicio;
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
