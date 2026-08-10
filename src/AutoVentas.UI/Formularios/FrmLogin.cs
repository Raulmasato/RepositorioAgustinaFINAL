using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios;

/// <summary>T02. Formulario de inicio de sesión, visible antes de acceder a cualquier parte del sistema.</summary>
public class FrmLogin : Form, IObservadorIdioma
{
    private readonly TextBox _txtUsuario = new() { Left = 140, Top = 40, Width = 200 };
    private readonly TextBox _txtClave = new() { Left = 140, Top = 75, Width = 200, UseSystemPasswordChar = true };
    private readonly Label _lblUsuario = new() { Left = 30, Top = 43, Width = 100 };
    private readonly Label _lblClave = new() { Left = 30, Top = 78, Width = 100 };
    private readonly Button _btnIngresar = new() { Left = 140, Top = 115, Width = 95 };
    private readonly Button _btnRegistrarse = new() { Left = 245, Top = 115, Width = 95 };
    private readonly SelectorIdioma _selectorIdioma = new() { Left = 140, Top = 155, Width = 200 };
    private readonly Button _btnTraducir = new() { Left = 345, Top = 153, Width = 65 };
    private readonly GestorAutenticacion _gestorAutenticacion = new();

    public FrmLogin()
    {
        Width = 420;
        Height = 260;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[]
        {
            _lblUsuario, _txtUsuario, _lblClave, _txtClave, _btnIngresar, _btnRegistrarse,
            _selectorIdioma, _btnTraducir
        });

        // --- DIAGNÓSTICO TEMPORAL: sacar este bloque una vez resuelto el problema de visualización ---
        MessageBox.Show(
            $"Controles agregados a esta ventana: {Controls.Count}" + Environment.NewLine +
            $"Tamaño ventana: {Width}x{Height}" + Environment.NewLine +
            $".NET en ejecución: {Environment.Version}" + Environment.NewLine +
            $"Modo alto DPI: {Application.HighDpiMode}" + Environment.NewLine +
            $"Ruta del ejecutable: {Environment.ProcessPath}",
            "DIAGNÓSTICO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        // --- FIN DIAGNÓSTICO TEMPORAL ---

        _btnIngresar.Click += BtnIngresar_Click;
        _btnRegistrarse.Click += BtnRegistrarse_Click;
        _btnTraducir.Click += BtnTraducir_Click;
        AcceptButton = _btnIngresar;

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
    }

    private void BtnIngresar_Click(object? sender, EventArgs e)
    {
        try
        {
            _gestorAutenticacion.IniciarSesion(_txtUsuario.Text.Trim(), _txtClave.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (AutoVentasException ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnRegistrarse_Click(object? sender, EventArgs e)
    {
        using var frmRegistro = new FrmRegistro();
        if (frmRegistro.ShowDialog(this) == DialogResult.OK)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.registroexitoso"),
                GestorIdioma.Instancia.Traducir("frm.registro"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// T05. Al tocar "Traducir" se toma el idioma elegido en el combo y se aplica a TODO el
    /// programa: GestorIdioma.CambiarIdioma (patrón Observer) recarga las traducciones desde
    /// la base de datos y notifica a cada formulario suscripto (no solo a este) para que
    /// refresque sus textos en caliente, sin necesidad de reiniciar la aplicación.
    /// </summary>
    private void BtnTraducir_Click(object? sender, EventArgs e)
    {
        if (_selectorIdioma.SelectedItem is not Idioma idiomaSeleccionado)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.seleccioneidioma"),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        GestorIdioma.Instancia.CambiarIdioma(idiomaSeleccionado.Codigo);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("frm.login");
        _lblUsuario.Text = t.Traducir("lbl.usuario");
        _lblClave.Text = t.Traducir("lbl.clave");
        _btnIngresar.Text = t.Traducir("btn.ingresar");
        _btnRegistrarse.Text = t.Traducir("btn.registrarse");
        _btnTraducir.Text = t.Traducir("btn.traducir");
    }
}
