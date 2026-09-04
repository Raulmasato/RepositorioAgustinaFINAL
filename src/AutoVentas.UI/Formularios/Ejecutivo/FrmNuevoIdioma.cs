namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Diálogo mínimo para dar de alta un idioma nuevo (código ISO corto + nombre visible).</summary>
internal partial class FrmNuevoIdioma : Form
{
    public string Codigo => _txtCodigo.Text.Trim().ToLowerInvariant();
    public string Nombre => _txtNombre.Text.Trim();

    public FrmNuevoIdioma()
    {
        InitializeComponent();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtCodigo.Text) || string.IsNullOrWhiteSpace(_txtNombre.Text))
        {
            MessageBox.Show(this, "Debe completar el código y el nombre del idioma.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
