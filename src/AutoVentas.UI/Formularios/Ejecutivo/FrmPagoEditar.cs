using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.Ejecutivo;

internal partial class FrmPagoEditar : Form, IObservadorIdioma
{
    private static readonly string[] MetodosPago = { "Efectivo", "Transferencia", "Tarjeta de crédito", "Tarjeta de débito", "Cheque" };

    private readonly GestorPagos _gestor = new();
    private readonly Pago? _original;

    public FrmPagoEditar(Pago? pago)
    {
        _original = pago;
        InitializeComponent();

        _cmbMetodo.Items.AddRange(MetodosPago);

        if (pago is not null)
        {
            _numMonto.Value = Math.Clamp(pago.Monto, _numMonto.Minimum, _numMonto.Maximum);
            _cmbMetodo.SelectedItem = pago.MetodoPago;
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
        _cmbContrato.Items.AddRange(new GestorContratos().ObtenerTodos().Cast<object>().ToArray());

        if (_original is not null)
        {
            _cmbContrato.SelectedItem = _cmbContrato.Items.Cast<Contrato>().FirstOrDefault(c => c.IdContrato == _original.IdContrato);
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbContrato.SelectedItem is not Contrato contrato || _cmbMetodo.SelectedItem is not string metodo)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.completetodosloscampos"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var usuario = SesionActual.Instancia.UsuarioLogueado!;
            var pago = _original ?? new Pago { FechaPago = DateTime.Now, IdUsuarioEjecutivo = usuario.IdUsuario };
            pago.IdContrato = contrato.IdContrato;
            pago.Monto = _numMonto.Value;
            pago.MetodoPago = metodo;

            if (_original is null) _gestor.Agregar(pago);
            else _gestor.Modificar(pago);

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
        Text = t.Traducir("menu.pagos");
        _lblContrato.Text = t.Traducir("menu.contratos");
        _lblMonto.Text = t.Traducir("lbl.monto");
        _lblMetodo.Text = t.Traducir("lbl.metodopago");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
