using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Tecnico;

internal partial class FrmMantenimientoEditar : Form, IObservadorIdioma
{
    private readonly GestorMantenimientos _gestor = new();
    private readonly Mantenimiento? _original;

    public FrmMantenimientoEditar(Mantenimiento? mantenimiento)
    {
        _original = mantenimiento;

        InitializeComponent();

        if (mantenimiento is not null)
        {
            _txtServicio.Text = mantenimiento.Servicio;
            _dtpFecha.Value = mantenimiento.FechaServicio;
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
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
