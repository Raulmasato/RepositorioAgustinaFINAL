using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Reportes (Ejecutivo). El contenido se genera automáticamente a partir
/// del tipo de reporte y el rango de fechas seleccionado.</summary>
public class FrmReportes : Form, IObservadorIdioma
{
    private readonly GestorReportes _gestor = new();

    private readonly DataGridView _grilla = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoGenerateColumns = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly FlowLayoutPanel _panelBotones = new() { Dock = DockStyle.Top, Height = 42, Padding = new Padding(6) };
    private readonly Button _btnNuevo = new() { AutoSize = true };
    private readonly Button _btnEditar = new() { AutoSize = true };
    private readonly Button _btnEliminar = new() { AutoSize = true };
    private readonly Button _btnRefrescar = new() { AutoSize = true };
    private readonly ControladorListadoCrud<Reporte> _controlador;

    public FrmReportes()
    {
        Width = 820;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reporte.IdReporte), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reporte.Titulo), HeaderText = "Título" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reporte.TipoReporte), HeaderText = "Tipo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reporte.FechaDesde), HeaderText = "Desde" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reporte.FechaHasta), HeaderText = "Hasta" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reporte.FechaGeneracion), HeaderText = "Generado" });

        _panelBotones.Controls.AddRange(new Control[] { _btnNuevo, _btnEditar, _btnEliminar, _btnRefrescar });
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _controlador = new ControladorListadoCrud<Reporte>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, r => _gestor.Eliminar(r.IdReporte));

        _btnNuevo.Click += (_, _) => _controlador.Nuevo();
        _btnEditar.Click += (_, _) => _controlador.Editar();
        _btnEliminar.Click += (_, _) => _controlador.EliminarSeleccionado();
        _btnRefrescar.Click += (_, _) => _controlador.Refrescar();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            _controlador.Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void AbrirAlta()
    {
        using var frm = new FrmReporteEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Reporte seleccionado)
    {
        using var frm = new FrmReporteEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.reportes");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}

internal class FrmReporteEditar : Form, IObservadorIdioma
{
    private readonly GestorReportes _gestor = new();
    private readonly Reporte? _original;

    private readonly Label _lblTitulo = new() { Left = 20, Top = 20, Width = 100 };
    private readonly TextBox _txtTitulo = new() { Left = 130, Top = 17, Width = 300 };
    private readonly Label _lblTipo = new() { Left = 20, Top = 55, Width = 100 };
    private readonly ComboBox _cmbTipo = new() { Left = 130, Top = 52, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblDesde = new() { Left = 20, Top = 90, Width = 100 };
    private readonly DateTimePicker _dtpDesde = new() { Left = 130, Top = 87, Width = 200 };
    private readonly Label _lblHasta = new() { Left = 20, Top = 125, Width = 100 };
    private readonly DateTimePicker _dtpHasta = new() { Left = 130, Top = 122, Width = 200 };
    private readonly TextBox _txtContenido = new()
    {
        Left = 20, Top = 160, Width = 410, Height = 150, Multiline = true, ReadOnly = true,
        ScrollBars = ScrollBars.Vertical, Font = new Font(FontFamily.GenericMonospace, 8)
    };
    private readonly Button _btnGuardar = new() { Left = 260, Top = 320, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 360, Top = 320, Width = 90 };

    public FrmReporteEditar(Reporte? reporte)
    {
        _original = reporte;
        Width = 460;
        Height = 400;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        _dtpDesde.Value = DateTime.Now.AddMonths(-1);

        Controls.AddRange(new Control[]
        {
            _lblTitulo, _txtTitulo, _lblTipo, _cmbTipo, _lblDesde, _dtpDesde, _lblHasta, _dtpHasta,
            _txtContenido, _btnGuardar, _btnCancelar
        });

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

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

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
