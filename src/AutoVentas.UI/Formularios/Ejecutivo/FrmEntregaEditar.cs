using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.Ejecutivo;

internal partial class FrmEntregaEditar : Form, IObservadorIdioma
{
    private readonly GestorEntregas _gestor = new();
    private readonly Entrega? _original;

    public FrmEntregaEditar(Entrega? entrega)
    {
        _original = entrega;
        InitializeComponent();
        _dtpFecha.Value = DateTime.Now.AddDays(3);

        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoEntrega>().Cast<object>().ToArray());

        if (entrega is not null)
        {
            _dtpFecha.Value = entrega.FechaEntrega;
            _txtLugar.Text = entrega.LugarEntrega;
            _cmbEstado.SelectedItem = entrega.Estado;
        }
        else
        {
            _cmbEstado.SelectedItem = EstadoEntrega.Pendiente;
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
        if (_cmbContrato.SelectedItem is not Contrato contrato)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.completetodosloscampos"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var usuario = SesionActual.Instancia.UsuarioLogueado!;
            var entrega = _original ?? new Entrega { IdUsuarioEjecutivo = usuario.IdUsuario };
            entrega.IdContrato = contrato.IdContrato;
            entrega.FechaEntrega = _dtpFecha.Value;
            entrega.LugarEntrega = _txtLugar.Text.Trim();
            entrega.Estado = (EstadoEntrega)_cmbEstado.SelectedItem!;

            if (_original is null) _gestor.Agregar(entrega);
            else _gestor.Modificar(entrega);

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
        Text = t.Traducir("menu.entregas");
        _lblContrato.Text = t.Traducir("menu.contratos");
        _lblFecha.Text = t.Traducir("lbl.fecha");
        _lblLugar.Text = t.Traducir("lbl.lugar");
        _lblEstado.Text = t.Traducir("lbl.estado");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
