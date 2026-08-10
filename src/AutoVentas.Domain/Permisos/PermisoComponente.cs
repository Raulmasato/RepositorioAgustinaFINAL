namespace AutoVentas.Domain.Permisos;

/// <summary>
/// T04. Gestión de Perfiles de Usuario — patrón Composite.
/// Componente base del árbol de permisos: tanto un permiso atómico (una funcionalidad puntual)
/// como un permiso compuesto (que agrupa otros permisos) se tratan de forma uniforme.
/// Esta clase vive en Domain, sin ninguna dependencia de la UI: el recorrido recursivo para
/// poblar un TreeView se hace en la capa de presentación, consumiendo <see cref="Hijos"/>.
/// </summary>
public abstract class PermisoComponente
{
    public int IdPermiso { get; }
    public string Codigo { get; }
    public string Nombre { get; }

    protected PermisoComponente(int idPermiso, string codigo, string nombre)
    {
        IdPermiso = idPermiso;
        Codigo = codigo;
        Nombre = nombre;
    }

    public virtual IReadOnlyList<PermisoComponente> Hijos => Array.Empty<PermisoComponente>();

    /// <summary>Devuelve, recorriendo el árbol, todos los permisos atómicos contenidos.</summary>
    public abstract IEnumerable<PermisoAtomico> ObtenerHojas();

    /// <summary>Indica si este componente (o alguno de sus hijos) representa el código dado.</summary>
    public bool Contiene(string codigo)
    {
        if (Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)) return true;
        return Hijos.Any(h => h.Contiene(codigo));
    }
}

/// <summary>Permiso atómico: representa una única funcionalidad (ej. "Crear vehiculo").</summary>
public sealed class PermisoAtomico : PermisoComponente
{
    public PermisoAtomico(int idPermiso, string codigo, string nombre) : base(idPermiso, codigo, nombre) { }

    public override IEnumerable<PermisoAtomico> ObtenerHojas()
    {
        yield return this;
    }
}

/// <summary>Permiso compuesto: agrupa otros permisos (atómicos o compuestos), ej. un perfil de rol.</summary>
public sealed class PermisoCompuesto : PermisoComponente
{
    private readonly List<PermisoComponente> _hijos = new();

    public PermisoCompuesto(int idPermiso, string codigo, string nombre) : base(idPermiso, codigo, nombre) { }

    public override IReadOnlyList<PermisoComponente> Hijos => _hijos;

    public void Agregar(PermisoComponente hijo) => _hijos.Add(hijo);

    public override IEnumerable<PermisoAtomico> ObtenerHojas() => Hijos.SelectMany(h => h.ObtenerHojas());
}
