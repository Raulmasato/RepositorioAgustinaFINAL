using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.Ejecutivo;

internal partial class FrmContratoEditar : Form, IObservadorIdioma
{
    private readonly GestorContratos _gestor = new();
    private readonly Contrato? _original;

    public FrmContratoEditar(Contrato? contrato)
    {
        _original = contrato;
        InitializeComponent();

        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoContrato>().Cast<object>().ToArray());
        _numPrecio.Value = contrato is not null ? Math.Clamp(contrato.Precio, _numPrecio.Minimum, _numPrecio.Maximum) : 0;
        _cmbEstado.SelectedItem = contrato?.Estado ?? EstadoContrato.Vigente;

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
        _cmbPresupuesto.Items.Add("(Ninguno)");
        _cmbPresupuesto.Items.AddRange(new GestorPresupuestos().ObtenerTodos().Cast<object>().ToArray());

        if (_original is not null)
        {
            _cmbVehiculo.SelectedItem = _cmbVehiculo.Items.Cast<Vehiculo>().FirstOrDefault(v => v.IdVehiculo == _original.IdVehiculo);
            _cmbCliente.SelectedItem = _cmbCliente.Items.Cast<Cliente>().FirstOrDefault(c => c.IdCliente == _original.IdCliente);
            _cmbPresupuesto.SelectedItem = _original.IdPresupuesto is int idP
                ? _cmbPresupuesto.Items.OfType<Presupuesto>().FirstOrDefault(p => p.IdPresupuesto == idP)
                : _cmbPresupuesto.Items[0];
        }
        else
        {
            _cmbPresupuesto.SelectedIndex = 0;
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
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
