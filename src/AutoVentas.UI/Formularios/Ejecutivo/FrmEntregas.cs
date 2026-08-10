using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Entregas (Ejecutivo). Una entrega &lt;&lt;include&gt;&gt; la gestión de pagos.</summary>
public class FrmEntregas : FormListadoBase<Entrega>
{
    private readonly GestorEntregas _gestor = new();

    protected override string ClaveTituloIdioma => "menu.entregas";

    protected override void ConfigurarColumnas(DataGridView g)
    {
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Entrega.IdEntrega), HeaderText = "Id", Width = 50 });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Entrega.ContratoDescripcion), HeaderText = "Contrato" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Entrega.FechaEntrega), HeaderText = "Fecha" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Entrega.LugarEntrega), HeaderText = "Lugar" });
        g.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Entrega.Estado), HeaderText = "Estado" });
    }

    protected override List<Entrega> ObtenerDatos() => _gestor.ObtenerTodos();

    protected override void AbrirAlta()
    {
        using var frm = new FrmEntregaEditar(null);
        frm.ShowDialog(this);
    }

    protected override void AbrirEdicion(Entrega seleccionado)
    {
        using var frm = new FrmEntregaEditar(seleccionado);
        frm.ShowDialog(this);
    }

    protected override void Eliminar(Entrega seleccionado) => _gestor.Eliminar(seleccionado.IdEntrega);
}

internal class FrmEntregaEditar : Form, IObservadorIdioma
{
    private readonly GestorEntregas _gestor = new();
    private readonly Entrega? _original;

    private readonly Label _lblContrato = new() { Left = 20, Top = 20, Width = 100 };
    private readonly ComboBox _cmbContrato = new() { Left = 130, Top = 17, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblFecha = new() { Left = 20, Top = 55, Width = 100 };
    private readonly DateTimePicker _dtpFecha = new() { Left = 130, Top = 52, Width = 220 };
    private readonly Label _lblLugar = new() { Left = 20, Top = 90, Width = 100 };
    private readonly TextBox _txtLugar = new() { Left = 130, Top = 87, Width = 220 };
    private readonly Label _lblEstado = new() { Left = 20, Top = 125, Width = 100 };
    private readonly ComboBox _cmbEstado = new() { Left = 130, Top = 122, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 165, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 165, Width = 90 };

    public FrmEntregaEditar(Entrega? entrega)
    {
        _original = entrega;
        Width = 400;
        Height = 250;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        _dtpFecha.Value = DateTime.Now.AddDays(3);

        Controls.AddRange(new Control[] { _lblContrato, _cmbContrato, _lblFecha, _dtpFecha, _lblLugar, _txtLugar, _lblEstado, _cmbEstado, _btnGuardar, _btnCancelar });

        _cmbContrato.Items.AddRange(new GestorContratos().ObtenerTodos().Cast<object>().ToArray());
        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoEntrega>().Cast<object>().ToArray());

        if (entrega is not null)
        {
            _cmbContrato.SelectedItem = _cmbContrato.Items.Cast<Contrato>().FirstOrDefault(c => c.IdContrato == entrega.IdContrato);
            _dtpFecha.Value = entrega.FechaEntrega;
            _txtLugar.Text = entrega.LugarEntrega;
            _cmbEstado.SelectedItem = entrega.Estado;
        }
        else
        {
            _cmbEstado.SelectedItem = EstadoEntrega.Pendiente;
        }

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
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
