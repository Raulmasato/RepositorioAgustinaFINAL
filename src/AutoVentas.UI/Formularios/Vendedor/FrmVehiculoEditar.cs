using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Vendedor;

internal partial class FrmVehiculoEditar : Form, IObservadorIdioma
{
    private readonly GestorVehiculos _gestor = new();
    private readonly Vehiculo? _original;

    public FrmVehiculoEditar(Vehiculo? vehiculo)
    {
        _original = vehiculo;

        InitializeComponent();

        if (vehiculo is not null)
        {
            _txtMarca.Text = vehiculo.Marca;
            _txtModelo.Text = vehiculo.Modelo;
            _txtColor.Text = vehiculo.Color;
            if (vehiculo.Anio is int anio) _numAnio.Value = Math.Clamp(anio, (int)_numAnio.Minimum, (int)_numAnio.Maximum);
            if (vehiculo.Precio is decimal precio) _numPrecio.Value = Math.Clamp(precio, _numPrecio.Minimum, _numPrecio.Maximum);
            _chkDisponible.Checked = vehiculo.Disponible;
        }

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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
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
