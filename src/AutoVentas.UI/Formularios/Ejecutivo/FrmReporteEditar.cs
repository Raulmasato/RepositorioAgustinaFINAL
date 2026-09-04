using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.Ejecutivo;

internal partial class FrmReporteEditar : Form, IObservadorIdioma
{
    private readonly GestorReportes _gestor = new();
    private readonly Reporte? _original;

    public FrmReporteEditar(Reporte? reporte)
    {
        _original = reporte;
        InitializeComponent();
        _dtpDesde.Value = DateTime.Now.AddMonths(-1);

        _cmbTipo.Items.AddRange(Enum.GetValues<TipoReporte>().Cast<object>().ToArray());

        if (reporte is not null)
        {
            _txtTitulo.Text = reporte.Titulo;
            _cmbTipo.SelectedItem = reporte.TipoReporte;
            _dtpDesde.Value = reporte.FechaDesde;
            _dtpHasta.Value = reporte.FechaHasta;
            _txtContenido.Text = reporte.Contenido;
        }
        else
        {
            _cmbTipo.SelectedItem = TipoReporte.Ventas;
        }

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtTitulo.Text))
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.completetodosloscampos"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var usuario = SesionActual.Instancia.UsuarioLogueado!;
            var reporte = _original ?? new Reporte { IdUsuarioEjecutivo = usuario.IdUsuario, FechaGeneracion = DateTime.Now };
            reporte.Titulo = _txtTitulo.Text.Trim();
            reporte.TipoReporte = (TipoReporte)_cmbTipo.SelectedItem!;
            reporte.FechaDesde = _dtpDesde.Value.Date;
            reporte.FechaHasta = _dtpHasta.Value.Date;

            if (_original is null) _gestor.Agregar(reporte);
            else _gestor.Modificar(reporte);

            _txtContenido.Text = reporte.Contenido;
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
        Text = t.Traducir("menu.reportes");
        _lblTitulo.Text = t.Traducir("lbl.titulo");
        _lblTipo.Text = t.Traducir("lbl.tipo");
        _lblDesde.Text = t.Traducir("lbl.desde");
        _lblHasta.Text = t.Traducir("lbl.hasta");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
