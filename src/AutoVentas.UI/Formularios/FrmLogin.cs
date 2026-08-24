using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios;

/// <summary>T02. Formulario de inicio de sesión, visible antes de acceder a cualquier parte del sistema.</summary>
public partial class FrmLogin : Form, IObservadorIdioma
{
    private readonly GestorAutenticacion _gestorAutenticacion = new();

    public FrmLogin()
    {
        InitializeComponent();

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
    private void Traducir_Click(object? sender, EventArgs e)
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
        _traducir.Text = t.Traducir("btn.traducir");
    }
}
