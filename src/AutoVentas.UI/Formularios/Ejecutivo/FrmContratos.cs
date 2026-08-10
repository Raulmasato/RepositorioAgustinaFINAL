using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Contratos (Ejecutivo). Puede originarse en un Presupuesto (&lt;&lt;include&gt;&gt;).</summary>
public class FrmContratos : FormListadoBase<Contrato>
{
    private readonly GestorContratos _gestor = new();

    protected override string ClaveTituloIdioma => "menu.contratos";

    protected override void ConfigurarColumnas(DataGridView g)
    {
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.IdContrato), HeaderText = "Id", Width = 50 });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.VehiculoDescripcion), HeaderText = "Vehículo" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.ClienteNombreCompleto), HeaderText = "Cliente" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.FechaContrato), HeaderText = "Fecha" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.Precio), HeaderText = "Precio" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.Estado), HeaderText = "Estado" });
    }

    protected override List<Contrato> ObtenerDatos() => _gestor.ObtenerTodos();

    protected override void AbrirAlta()
    {
        using var frm = new FrmContratoEditar(null);
        frm.ShowDialog(this);
    }

    protected override void AbrirEdicion(Contrato seleccionado)
    {
        using var frm = new FrmContratoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    protected override void Eliminar(Contrato seleccionado) => _gestor.Eliminar(seleccionado.IdContrato);
}

internal class FrmContratoEditar : Form, IObservadorIdioma
{
    private readonly GestorContratos _gestor = new();
    private readonly Contrato? _original;

    private readonly Label _lblVehiculo = new() { Left = 20, Top = 20, Width = 100 };
    private readonly ComboBox _cmbVehiculo = new() { Left = 130, Top = 17, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblCliente = new() { Left = 20, Top = 55, Width = 100 };
    private readonly ComboBox _cmbCliente = new() { Left = 130, Top = 52, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblPresupuesto = new() { Left = 20, Top = 90, Width = 100 };
    private readonly ComboBox _cmbPresupuesto = new() { Left = 130, Top = 87, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblPrecio = new() { Left = 20, Top = 125, Width = 100 };
    private readonly NumericUpDown _numPrecio = new() { Left = 130, Top = 122, Width = 150, Maximum = 100_000_000, DecimalPlaces = 2 };
    private readonly Label _lblEstado = new() { Left = 20, Top = 160, Width = 100 };
    private readonly ComboBox _cmbEstado = new() { Left = 130, Top = 157, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 200, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 200, Width = 90 };

    public FrmContratoEditar(Contrato? contrato)
    {
        _original = contrato;
        Width = 400;
        Height = 290;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[]
        {
            _lblVehiculo, _cmbVehiculo, _lblCliente, _cmbCliente, _lblPresupuesto, _cmbPresupuesto,
            _lblPrecio, _numPrecio, _lblEstado, _cmbEstado, _btnGuardar, _btnCancelar
        });

        _cmbVehiculo.Items.AddRange(new GestorVehiculos().ObtenerTodos().Cast<object>().ToArray());
        _cmbCliente.Items.AddRange(new GestorClientes().ObtenerTodos().Cast<object>().ToArray());
        _cmbPresupuesto.Items.Add("(Ninguno)");
        _cmbPresupuesto.Items.AddRange(new GestorPresupuestos().ObtenerTodos().Cast<object>().ToArray());
        _cmbPresupuesto.DisplayMember = string.Empty;
        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoContrato>().Cast<object>().ToArray());

        if (contrato is not null)
        {
            _cmbVehiculo.SelectedItem = _cmbVehiculo.Items.Cast<Vehiculo>().FirstOrDefault(v => v.IdVehiculo == contrato.IdVehiculo);
            _cmbCliente.SelectedItem = _cmbCliente.Items.Cast<Cliente>().FirstOrDefault(c => c.IdCliente == contrato.IdCliente);
            _cmbPresupuesto.SelectedItem = contrato.IdPresupuesto is int idP
                ? _cmbPresupuesto.Items.OfType<Presupuesto>().FirstOrDefault(p => p.IdPresupuesto == idP)
                : _cmbPresupuesto.Items[0];
            _numPrecio.Value = Math.Clamp(contrato.Precio, _numPrecio.Minimum, _numPrecio.Maximum);
            _cmbEstado.SelectedItem = contrato.Estado;
        }
        else
        {
            _cmbPresupuesto.SelectedIndex = 0;
            _cmbEstado.SelectedItem = EstadoContrato.Vigente;
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
            var contrato = _original ?? new Contrato { FechaContrato = DateTime.Now, IdUsuarioEjecutivo = usuario.IdUsuario };
            contrato.IdVehiculo = vehiculo.IdVehiculo;
            contrato.IdCliente = cliente.IdCliente;
            contrato.IdPresupuesto = _cmbPresupuesto.SelectedItem is Presupuesto presupuesto ? presupuesto.IdPresupuesto : null;
            contrato.Precio = _numPrecio.Value;
            contrato.Estado = (EstadoContrato)_cmbEstado.SelectedItem!;

            if (_original is null) _gestor.Agregar(contrato);
            else _gestor.Modificar(contrato);

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
        Text = t.Traducir("menu.contratos");
        _lblVehiculo.Text = t.Traducir("lbl.vehiculo");
        _lblCliente.Text = t.Traducir("lbl.cliente");
        _lblPresupuesto.Text = t.Traducir("menu.presupuestos");
        _lblPrecio.Text = t.Traducir("lbl.precio");
        _lblEstado.Text = t.Traducir("lbl.estado");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
