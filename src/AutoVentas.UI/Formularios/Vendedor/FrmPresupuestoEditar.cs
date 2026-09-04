using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.Vendedor;

internal partial class FrmPresupuestoEditar : Form, IObservadorIdioma
{
    private readonly GestorPresupuestos _gestor = new();
    private readonly Presupuesto? _original;

    public FrmPresupuestoEditar(Presupuesto? presupuesto)
    {
        _original = presupuesto;

        InitializeComponent();

        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoPresupuesto>().Cast<object>().ToArray());
        _numMonto.Value = presupuesto is not null ? Math.Clamp(presupuesto.Monto, _numMonto.Minimum, _numMonto.Maximum) : 0;
        _cmbEstado.SelectedItem = presupuesto?.Estado ?? EstadoPresupuesto.Pendiente;

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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
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
