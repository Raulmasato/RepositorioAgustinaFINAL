using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.PortalCliente;
using AutoVentas.UI.Formularios.Comunes;
using AutoVentas.UI.Formularios.Ejecutivo;
using AutoVentas.UI.Formularios.Tecnico;
using AutoVentas.UI.Formularios.Vendedor;

namespace AutoVentas.UI.Formularios;

/// <summary>
/// Formulario visible para todos los roles del sistema una vez logueados. Desde acá el
/// usuario navega, con un único botón, al menú correspondiente a su rol (Ejecutivo,
/// Vendedor, Técnico o Cliente), evitando exponer opciones que no le corresponden.
/// </summary>
public partial class FrmPrincipal : Form, IObservadorIdioma
{
    /// <summary>Le indica a Program.cs si, al cerrarse este formulario, corresponde volver a
    /// mostrar el login (cierre de sesión explícito) en lugar de terminar la aplicación.</summary>
    public static bool SolicitarNuevoLogin;

    public FrmPrincipal()
    {
        InitializeComponent();

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
    }

    private void BtnIrAlMenu_Click(object? sender, EventArgs e)
    {
        var usuario = SesionActual.Instancia.UsuarioLogueado;
        if (usuario is null) return;

        Form menu = usuario.Rol switch
        {
            NombreRol.Ejecutivo => new FrmMenuEjecutivo(),
            NombreRol.Vendedor => new FrmMenuVendedor(),
            NombreRol.Tecnico => new FrmMenuTecnico(),
            NombreRol.Cliente => new FrmMenuCliente(),
            _ => throw new InvalidOperationException("Rol de usuario no soportado.")
        };

        using (menu)
        {
            menu.ShowDialog(this);
        }
    }

    private void BtnCerrarSesion_Click(object? sender, EventArgs e)
    {
        new GestorAutenticacion().CerrarSesion();
        SolicitarNuevoLogin = true;
        Close();
    }

    private void BtnAyuda_Click(object? sender, EventArgs e)
    {
        new FrmAyuda().Show(this);
    }

    /// <summary>T05. Aplica a TODO el programa el idioma elegido en el combo (el combo solo
    /// selecciona; este botón es el que efectivamente dispara GestorIdioma.CambiarIdioma).</summary>
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
        Text = t.Traducir("frm.principal");
        var usuario = SesionActual.Instancia.UsuarioLogueado;
        _lblBienvenida.Text = usuario is null ? string.Empty : $"{usuario.NombreUsuario} ({usuario.Rol})";
        _btnIrAlMenu.Text = t.Traducir("btn.iralmenu");
        _btnCerrarSesion.Text = t.Traducir("btn.cerrarsesion");
        _lblIdioma.Text = t.Traducir("menu.idioma");
        _traducir.Text = t.Traducir("btn.traducir");
        _btnAyuda.Text = t.Traducir("menu.ayuda");
    }
}
