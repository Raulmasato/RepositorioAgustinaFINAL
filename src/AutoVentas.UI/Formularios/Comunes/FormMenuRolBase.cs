using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Comunes;

/// <summary>
/// T01. Base MDI para los cuatro menús por rol (Ejecutivo, Vendedor, Técnico, Cliente).
/// Cada gestión habilitada para el rol se abre como formulario hijo MDI, de forma que el
/// usuario pueda tener varias pantallas abiertas a la vez dentro de su propio menú.
/// </summary>
public abstract class FormMenuRolBase : Form, IObservadorIdioma
{
    private readonly MenuStrip _menuStrip = new();
    private readonly ToolStripMenuItem _menuOpciones = new();
    private readonly ToolStripMenuItem _menuAyuda = new();
    private readonly ToolStripMenuItem _menuVolver = new();
    private readonly ToolStripControlHost _hostSelectorIdioma;
    private readonly List<(ToolStripMenuItem Item, string Clave)> _itemsTraducibles = new();

    protected abstract string ClaveTituloIdioma { get; }
    protected abstract string ClaveMenuOpciones { get; }

    protected FormMenuRolBase()
    {
        Width = 1000;
        Height = 650;
        StartPosition = FormStartPosition.CenterScreen;
        IsMdiContainer = true;

        _hostSelectorIdioma = new ToolStripControlHost(new SelectorIdioma());

        _menuStrip.Items.Add(_menuOpciones);
        _menuStrip.Items.Add(_menuAyuda);
        _menuStrip.Items.Add(_menuVolver);
        _menuStrip.Items.Add(_hostSelectorIdioma);
        MainMenuStrip = _menuStrip;
        Controls.Add(_menuStrip);

        _menuAyuda.Click += (_, _) => new FrmAyuda().Show(this);
        _menuVolver.Click += (_, _) => Close();

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        Load += (_, _) => ActualizarIdioma();
    }

    /// <summary>Agrega una opción al menú principal que abre (o trae al frente) un formulario hijo MDI.</summary>
    protected void AgregarOpcion(string claveIdioma, Func<Form> crearFormulario)
    {
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

    public virtual void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir(ClaveTituloIdioma);
        _menuOpciones.Text = t.Traducir(ClaveMenuOpciones);
        _menuAyuda.Text = t.Traducir("menu.ayuda");
        _menuVolver.Text = t.Traducir("btn.volver");
        foreach (var (item, clave) in _itemsTraducibles)
        {
            item.Text = t.Traducir(clave);
        }
    }
}
