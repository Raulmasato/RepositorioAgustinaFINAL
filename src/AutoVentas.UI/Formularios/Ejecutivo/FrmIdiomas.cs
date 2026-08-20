using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>
/// T05. Gestión de Múltiples Idiomas — pantalla para incorporar idiomas nuevos y editar sus
/// leyendas DESDE EL SISTEMA (no hace falta tocar la base de datos a mano). Al guardar, si el
/// idioma editado es el que está activo en ese momento, se vuelve a aplicar automáticamente
/// (GestorIdioma.CambiarIdioma) para reflejar los cambios sin reiniciar la aplicación.
/// </summary>
public class FrmIdiomas : Form, IObservadorIdioma
{
    private readonly RepositorioIdiomas _repositorio = new();

    private readonly ComboBox _cmbIdioma = new() { Left = 10, Top = 10, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button _btnNuevoIdioma = new() { Left = 240, Top = 8, Width = 130 };
    private readonly Button _btnGuardar = new() { Left = 380, Top = 8, Width = 130 };

    private readonly DataGridView _grilla = new()
    {
        Top = 44, Left = 0, Dock = DockStyle.Fill,
        AllowUserToAddRows = false, AllowUserToDeleteRows = false,
        SelectionMode = DataGridViewSelectionMode.CellSelect,
        AutoGenerateColumns = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly Panel _panelSuperior = new() { Dock = DockStyle.Top, Height = 44 };

    public FrmIdiomas()
    {
        Width = 700;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { Name = "Clave", HeaderText = "Clave", ReadOnly = true, FillWeight = 30 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { Name = "Valor", HeaderText = "Texto en este idioma", FillWeight = 70 });

        _panelSuperior.Controls.AddRange(new Control[] { _cmbIdioma, _btnNuevoIdioma, _btnGuardar });
        Controls.Add(_grilla);
        Controls.Add(_panelSuperior);

        _cmbIdioma.DisplayMember = nameof(Idioma.Nombre);
        _cmbIdioma.SelectedIndexChanged += (_, _) => CargarGrilla();
        _btnNuevoIdioma.Click += BtnNuevoIdioma_Click;
        _btnGuardar.Click += BtnGuardar_Click;

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            CargarCombo();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void CargarCombo()
    {
        var idiomaAnterior = _cmbIdioma.SelectedItem as Idioma;
        _cmbIdioma.Items.Clear();
        _cmbIdioma.Items.AddRange(_repositorio.ObtenerTodos().Cast<object>().ToArray());

        var aReseleccionar = idiomaAnterior is null
            ? null
            : _cmbIdioma.Items.Cast<Idioma>().FirstOrDefault(i => i.IdIdioma == idiomaAnterior.IdIdioma);

        _cmbIdioma.SelectedItem = aReseleccionar ?? _cmbIdioma.Items.Cast<Idioma>().FirstOrDefault();
    }

    private void CargarGrilla()
    {
        _grilla.Rows.Clear();
        if (_cmbIdioma.SelectedItem is not Idioma idioma) return;

        var traducciones = _repositorio.ObtenerTraducciones(idioma.Codigo);
        var claves = _repositorio.ObtenerClavesConocidas();

        foreach (var clave in claves)
        {
            traducciones.TryGetValue(clave, out var valor);
            _grilla.Rows.Add(clave, valor ?? string.Empty);
        }
    }

    private void BtnNuevoIdioma_Click(object? sender, EventArgs e)
    {
        using var frm = new FrmNuevoIdioma();
        if (frm.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            var idNuevo = _repositorio.AgregarIdioma(frm.Codigo, frm.Nombre);
            GestorIdioma.Instancia.Inicializar(GestorIdioma.Instancia.CodigoIdiomaActual);
            CargarCombo();
            _cmbIdioma.SelectedItem = _cmbIdioma.Items.Cast<Idioma>().FirstOrDefault(i => i.IdIdioma == idNuevo);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbIdioma.SelectedItem is not Idioma idioma) return;

        try
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                var clave = Convert.ToString(fila.Cells["Clave"].Value) ?? string.Empty;
                var valor = Convert.ToString(fila.Cells["Valor"].Value) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(clave)) continue;
                _repositorio.GuardarTraduccion(idioma.IdIdioma, clave, valor);
            }

            // Si el idioma editado es el que está activo, se vuelve a aplicar para que se vean
            // los cambios recién guardados sin tener que reiniciar la aplicación (T05, Observer).
            if (idioma.Codigo == GestorIdioma.Instancia.CodigoIdiomaActual)
            {
                GestorIdioma.Instancia.CambiarIdioma(idioma.Codigo);
            }

            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.idiomaguardado"),
                "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    public void ActualizarIdioma()
    {
        Text = GestorIdioma.Instancia.Traducir("menu.idiomas");
        _btnNuevoIdioma.Text = GestorIdioma.Instancia.Traducir("btn.nuevoidioma");
        _btnGuardar.Text = GestorIdioma.Instancia.Traducir("btn.guardar");
    }
}

/// <summary>Diálogo mínimo para dar de alta un idioma nuevo (código ISO corto + nombre visible).</summary>
internal class FrmNuevoIdioma : Form
{
    public string Codigo => _txtCodigo.Text.Trim().ToLowerInvariant();
    public string Nombre => _txtNombre.Text.Trim();

    private readonly Label _lblCodigo = new() { Left = 20, Top = 20, Width = 100 };
    private readonly TextBox _txtCodigo = new() { Left = 130, Top = 17, Width = 100, MaxLength = 10 };
    private readonly Label _lblNombre = new() { Left = 20, Top = 55, Width = 100 };
    private readonly TextBox _txtNombre = new() { Left = 130, Top = 52, Width = 200 };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 90, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 90, Width = 90 };

    public FrmNuevoIdioma()
    {
        Text = "Nuevo idioma";
        Width = 380;
        Height = 180;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        _lblCodigo.Text = "Código (ej: it)";
        _lblNombre.Text = "Nombre";
        _btnGuardar.Text = "Guardar";
        _btnCancelar.Text = "Cancelar";

        Controls.AddRange(new Control[] { _lblCodigo, _txtCodigo, _lblNombre, _txtNombre, _btnGuardar, _btnCancelar });

        _btnGuardar.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(_txtCodigo.Text) || string.IsNullOrWhiteSpace(_txtNombre.Text))
            {
                MessageBox.Show(this, "Debe completar el código y el nombre del idioma.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        };
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
    }
}
