using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Vendedor;

/// <summary>Gestión de Presupuestos (Vendedor). Un presupuesto aprobado puede dar origen a un Contrato.</summary>
public class FrmPresupuestos : FormListadoBase<Presupuesto>
{
    private readonly GestorPresupuestos _gestor = new();

    protected override string ClaveTituloIdioma => "menu.presupuestos";

    protected override void ConfigurarColumnas(DataGridView g)
    {
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Presupuesto.IdPresupuesto), HeaderText = "Id", Width = 50 });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Presupuesto.VehiculoDescripcion), HeaderText = "Vehículo" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Presupuesto.ClienteNombreCompleto), HeaderText = "Cliente" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Presupuesto.FechaPresupuesto), HeaderText = "Fecha" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Presupuesto.Monto), HeaderText = "Monto" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Presupuesto.Estado), HeaderText = "Estado" });
    }

    protected override List<Presupuesto> ObtenerDatos() => _gestor.ObtenerTodos();

    protected override void AbrirAlta()
    {
        using var frm = new FrmPresupuestoEditar(null);
        frm.ShowDialog(this);
    }

    protected override void AbrirEdicion(Presupuesto seleccionado)
    {
        using var frm = new FrmPresupuestoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    protected override void Eliminar(Presupuesto seleccionado) => _gestor.Eliminar(seleccionado.IdPresupuesto);
}

internal class FrmPresupuestoEditar : Form, IObservadorIdioma
{
    private readonly GestorPresupuestos _gestor = new();
    private readonly Presupuesto? _original;

    private readonly Label _lblVehiculo = new() { Left = 20, Top = 20, Width = 100 };
    private readonly ComboBox _cmbVehiculo = new() { Left = 130, Top = 17, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblCliente = new() { Left = 20, Top = 55, Width = 100 };
    private readonly ComboBox _cmbCliente = new() { Left = 130, Top = 52, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblMonto = new() { Left = 20, Top = 90, Width = 100 };
    private readonly NumericUpDown _numMonto = new() { Left = 130, Top = 87, Width = 150, Maximum = 100_000_000, DecimalPlaces = 2 };
    private readonly Label _lblEstado = new() { Left = 20, Top = 125, Width = 100 };
    private readonly ComboBox _cmbEstado = new() { Left = 130, Top = 122, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 165, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 165, Width = 90 };

    public FrmPresupuestoEditar(Presupuesto? presupuesto)
    {
        _original = presupuesto;
        Width = 400;
        Height = 250;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[]
        {
            _lblVehiculo, _cmbVehiculo, _lblCliente, _cmbCliente, _lblMonto, _numMonto,
            _lblEstado, _cmbEstado, _btnGuardar, _btnCancelar
        });

        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoPresupuesto>().Cast<object>().ToArray());
        _numMonto.Value = presupuesto is not null ? Math.Clamp(presupuesto.Monto, _numMonto.Minimum, _numMonto.Maximum) : 0;
        _cmbEstado.SelectedItem = presupuesto?.Estado ?? EstadoPresupuesto.Pendiente;

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);

        // Las consultas a la base de datos se difieren a Load (y no van en el constructor)
        // para que el diseñador de Visual Studio pueda instanciar este formulario sin conexión.
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
            var usuario = SesionActual.Instancia.UsuarioLogueado!;
            var presupuesto = _original ?? new Presupuesto { FechaPresupuesto = DateTime.Now, IdUsuarioVendedor = usuario.IdUsuario };
            presupuesto.IdVehiculo = vehiculo.IdVehiculo;
            presupuesto.IdCliente = cliente.IdCliente;
            presupuesto.Monto = _numMonto.Value;
            presupuesto.Estado = (EstadoPresupuesto)_cmbEstado.SelectedItem!;

            if (_original is null) _gestor.Agregar(presupuesto);
            else _gestor.Modificar(presupuesto);

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
        Text = t.Traducir("menu.presupuestos");
        _lblVehiculo.Text = t.Traducir("lbl.vehiculo");
        _lblCliente.Text = t.Traducir("lbl.cliente");
        _lblMonto.Text = t.Traducir("lbl.monto");
        _lblEstado.Text = t.Traducir("lbl.estado");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
