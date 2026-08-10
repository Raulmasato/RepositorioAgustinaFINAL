using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Permisos;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Permisos;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>
/// T04. Gestión de Perfiles de Usuario. Muestra el árbol de permisos (patrón Composite)
/// en un TreeView, poblado mediante una función recursiva, y permite tildar/destildar qué
/// permisos compuestos/atómicos tiene asignados directamente cada rol.
/// </summary>
public class FrmPermisos : Form, IObservadorIdioma
{
    private readonly ServicioPermisos _servicioPermisos = new();
    private readonly RepositorioRoles _repositorioRoles = new();

    private readonly Label _lblRol = new() { Left = 10, Top = 14, Width = 60 };
    private readonly ComboBox _cmbRol = new() { Left = 75, Top = 10, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TreeView _arbol = new() { Dock = DockStyle.Fill, CheckBoxes = true };
    private readonly Panel _panelSuperior = new() { Dock = DockStyle.Top, Height = 42 };
    private readonly Button _btnGuardar = new() { Dock = DockStyle.Bottom, Height = 34 };

    private bool _actualizandoDesdeCodigo;

    public FrmPermisos()
    {
        Width = 520;
        Height = 600;
        StartPosition = FormStartPosition.CenterParent;

        _panelSuperior.Controls.AddRange(new Control[] { _lblRol, _cmbRol });
        Controls.Add(_arbol);
        Controls.Add(_btnGuardar);
        Controls.Add(_panelSuperior);

        _cmbRol.Items.AddRange(_repositorioRoles.ObtenerTodos().Cast<object>().ToArray());
        _cmbRol.DisplayMember = nameof(Rol.Nombre);
        _cmbRol.SelectedIndexChanged += (_, _) => CargarArbolParaRolSeleccionado();

        _arbol.AfterCheck += Arbol_AfterCheck;
        _btnGuardar.Click += (_, _) => GuardarCambios();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            if (_cmbRol.Items.Count > 0) _cmbRol.SelectedIndex = 0;
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void CargarArbolParaRolSeleccionado()
    {
        if (_cmbRol.SelectedItem is not Rol rol) return;

        _actualizandoDesdeCodigo = true;
        _arbol.Nodes.Clear();

        var raices = _servicioPermisos.ObtenerArbolCompleto();
        var codigosDirectos = _servicioPermisos.ObtenerCodigosDirectosDelRol(rol.IdRol);

        foreach (var raiz in raices)
        {
            _arbol.Nodes.Add(ConstruirNodo(raiz, codigosDirectos));
        }

        _arbol.ExpandAll();
        _actualizandoDesdeCodigo = false;
    }

    /// <summary>Recorrido recursivo del árbol Composite de permisos, construyendo el TreeNode
    /// equivalente para el TreeView (tal como pide la especificación T04).</summary>
    private static TreeNode ConstruirNodo(PermisoComponente componente, HashSet<string> codigosDirectosDelRol)
    {
        var nodo = new TreeNode($"[{componente.Codigo}] {componente.Nombre}")
        {
            Tag = componente,
            Checked = codigosDirectosDelRol.Contains(componente.Codigo)
        };

        foreach (var hijo in componente.Hijos)
        {
            nodo.Nodes.Add(ConstruirNodo(hijo, codigosDirectosDelRol));
        }

        return nodo;
    }

    private void Arbol_AfterCheck(object? sender, TreeViewEventArgs e)
    {
        if (_actualizandoDesdeCodigo || e.Node is null) return;

        _actualizandoDesdeCodigo = true;
        MarcarDescendientes(e.Node, e.Node.Checked);
        _actualizandoDesdeCodigo = false;
    }

    private static void MarcarDescendientes(TreeNode nodo, bool marcado)
    {
        foreach (TreeNode hijo in nodo.Nodes)
        {
            hijo.Checked = marcado;
            MarcarDescendientes(hijo, marcado);
        }
    }

    private void GuardarCambios()
    {
        if (_cmbRol.SelectedItem is not Rol rol) return;

        void RecorrerYGuardar(TreeNode nodo)
        {
            if (nodo.Tag is PermisoComponente componente)
            {
                if (nodo.Checked) _servicioPermisos.AsignarPermisoARol(rol.IdRol, componente.IdPermiso);
                else _servicioPermisos.QuitarPermisoDeRol(rol.IdRol, componente.IdPermiso);
            }
            foreach (TreeNode hijo in nodo.Nodes) RecorrerYGuardar(hijo);
        }

        foreach (TreeNode raiz in _arbol.Nodes) RecorrerYGuardar(raiz);

        MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.permisosguardados"),
            GestorIdioma.Instancia.Traducir("menu.permisos"), MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.permisos");
        _lblRol.Text = t.Traducir("lbl.rol");
        _btnGuardar.Text = t.Traducir("btn.guardar");
    }
}
