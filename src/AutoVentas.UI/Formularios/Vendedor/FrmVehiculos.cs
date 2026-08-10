using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Vendedor;

/// <summary>Gestión de Vehículos (alta de inventario a cargo del Vendedor).</summary>
public class FrmVehiculos : FormListadoBase<Vehiculo>
{
    private readonly GestorVehiculos _gestor = new();

    protected override string ClaveTituloIdioma => "menu.vehiculos";

    protected override void ConfigurarColumnas(DataGridView g)
    {
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.IdVehiculo), HeaderText = "Id", Width = 50 });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Marca), HeaderText = "Marca" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Modelo), HeaderText = "Modelo" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Color), HeaderText = "Color" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Anio), HeaderText = "Año" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Precio), HeaderText = "Precio" });
        g.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(Vehiculo.Disponible), HeaderText = "Disponible" });
    }

    protected override List<Vehiculo> ObtenerDatos() => _gestor.ObtenerTodos();

    protected override void AbrirAlta()
    {
        using var frm = new FrmVehiculoEditar(null);
        frm.ShowDialog(this);
    }

    protected override void AbrirEdicion(Vehiculo seleccionado)
    {
        using var frm = new FrmVehiculoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    protected override void Eliminar(Vehiculo seleccionado) => _gestor.Eliminar(seleccionado.IdVehiculo);
}

internal class FrmVehiculoEditar : Form, IObservadorIdioma
{
    private readonly GestorVehiculos _gestor = new();
    private readonly Vehiculo? _original;

    private readonly Label _lblMarca = new() { Left = 20, Top = 20, Width = 100 };
    private readonly TextBox _txtMarca = new() { Left = 130, Top = 17, Width = 200 };
    private readonly Label _lblModelo = new() { Left = 20, Top = 55, Width = 100 };
    private readonly TextBox _txtModelo = new() { Left = 130, Top = 52, Width = 200 };
    private readonly Label _lblColor = new() { Left = 20, Top = 90, Width = 100 };
    private readonly TextBox _txtColor = new() { Left = 130, Top = 87, Width = 200 };
    private readonly Label _lblAnio = new() { Left = 20, Top = 125, Width = 100 };
    private readonly NumericUpDown _numAnio = new() { Left = 130, Top = 122, Width = 100, Minimum = 1950, Maximum = 2100, Value = DateTime.Now.Year };
    private readonly Label _lblPrecio = new() { Left = 20, Top = 160, Width = 100 };
    private readonly NumericUpDown _numPrecio = new() { Left = 130, Top = 157, Width = 150, Maximum = 100_000_000, DecimalPlaces = 2 };
    private readonly CheckBox _chkDisponible = new() { Left = 130, Top = 195, Width = 200, Checked = true };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 235, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 235, Width = 90 };

    public FrmVehiculoEditar(Vehiculo? vehiculo)
    {
        _original = vehiculo;
        Width = 380;
        Height = 320;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[]
        {
            _lblMarca, _txtMarca, _lblModelo, _txtModelo, _lblColor, _txtColor,
            _lblAnio, _numAnio, _lblPrecio, _numPrecio, _chkDisponible, _btnGuardar, _btnCancelar
        });

        if (vehiculo is not null)
        {
            _txtMarca.Text = vehiculo.Marca;
            _txtModelo.Text = vehiculo.Modelo;
            _txtColor.Text = vehiculo.Color;
            if (vehiculo.Anio is int anio) _numAnio.Value = Math.Clamp(anio, (int)_numAnio.Minimum, (int)_numAnio.Maximum);
            if (vehiculo.Precio is decimal precio) _numPrecio.Value = Math.Clamp(precio, _numPrecio.Minimum, _numPrecio.Maximum);
            _chkDisponible.Checked = vehiculo.Disponible;
        }

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            var vehiculo = _original ?? new Vehiculo();
            vehiculo.Marca = _txtMarca.Text.Trim();
            vehiculo.Modelo = _txtModelo.Text.Trim();
            vehiculo.Color = _txtColor.Text.Trim();
            vehiculo.Anio = (int)_numAnio.Value;
            vehiculo.Precio = _numPrecio.Value;
            vehiculo.Disponible = _chkDisponible.Checked;

            if (_original is null) _gestor.Agregar(vehiculo);
            else _gestor.Modificar(vehiculo);

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
        Text = t.Traducir("menu.vehiculos");
        _lblMarca.Text = t.Traducir("lbl.marca");
        _lblModelo.Text = t.Traducir("lbl.modelo");
        _lblColor.Text = t.Traducir("lbl.color");
        _lblAnio.Text = t.Traducir("lbl.anio");
        _lblPrecio.Text = t.Traducir("lbl.precio");
        _chkDisponible.Text = t.Traducir("lbl.disponible");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
