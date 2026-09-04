using AutoVentas.Services.Idioma;
using AutoVentas.Services.Permisos;
using AutoVentas.Services.Seguridad;
using EntidadIdioma = AutoVentas.Domain.Entidades.Idioma;

namespace AutoVentas.UI.Formularios.Comunes;

/// <summary>
/// T01. Base MDI para los cuatro menús por rol (Ejecutivo, Vendedor, Técnico, Cliente).
/// Cada gestión habilitada para el rol se abre como formulario hijo MDI, de forma que el
/// usuario pueda tener varias pantallas abiertas a la vez dentro de su propio menú.
/// </summary>
public abstract partial class FormMenuRolBase : Form, IObservadorIdioma
{
    private readonly List<(ToolStripMenuItem Item, string Clave)> _itemsTraducibles = new();
    private readonly ServicioPermisos _servicioPermisos = new();

    protected abstract string ClaveTituloIdioma { get; }
    protected abstract string ClaveMenuOpciones { get; }

    protected FormMenuRolBase()
    {
        InitializeComponent();

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        Load += (_, _) => ActualizarIdioma();
    }

    private void MenuAyuda_Click(object? sender, EventArgs e)
    {
        new FrmAyuda().Show(this);
    }

    private void MenuVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// T04. Agrega una opción al menú principal que abre (o trae al frente) un formulario hijo
    /// MDI, respetando los permisos asignados al rol del usuario logueado: si se indica
    /// <paramref name="codigoPermiso"/> y el rol actual no lo tiene asignado (directamente o a
    /// través de un permiso compuesto que lo agrupe), la opción directamente no se agrega al
    /// menú. Sin <paramref name="codigoPermiso"/> la opción queda disponible para todo el rol
    /// (uso para funciones administrativas del propio menú, como Bitácora/Backup, que ya están
    /// acotadas por rol al no existir en los menús de los otros roles).
    /// </summary>
    protected void AgregarOpcion(string claveIdioma, Func<Form> crearFormulario, string? codigoPermiso = null)
    {
        if (codigoPermiso is not null)
        {
            var idRol = SesionActual.Instancia.UsuarioLogueado?.IdRol;
            if (idRol is null || !_servicioPermisos.RolTienePermiso(idRol.Value, codigoPermiso))
            {
                return;
            }
        }

        var item = new ToolStripMenuItem();
        item.Click += (_, _) =>
        {
            var hijo = crearFormulario();
            hijo.MdiParent = this;
            hijo.WindowState = FormWindowState.Maximized;
            hijo.Show();
        };
        _menuOpciones.DropDownItems.Add(item);
        _itemsTraducibles.Add((item, claveIdioma));
    }

    /// <summary>T05. El combo de idioma solo selecciona; este botón es el que aplica el
    /// cambio a TODO el programa (GestorIdioma.CambiarIdioma, patrón Observer).</summary>
    private void Traducir_Click(object? sender, EventArgs e)
    {
        if (_selectorIdioma.SelectedItem is not EntidadIdioma idiomaSeleccionado)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.seleccioneidioma"),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        GestorIdioma.Instancia.CambiarIdioma(idiomaSeleccionado.Codigo);
    }

    public virtual void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir(ClaveTituloIdioma);
        _menuOpciones.Text = t.Traducir(ClaveMenuOpciones);
        _menuAyuda.Text = t.Traducir("menu.ayuda");
        _menuVolver.Text = t.Traducir("btn.volver");
        _menuTraducir.Text = t.Traducir("btn.traducir");
        foreach (var (item, clave) in _itemsTraducibles)
        {
            item.Text = t.Traducir(clave);
        }
    }
}
