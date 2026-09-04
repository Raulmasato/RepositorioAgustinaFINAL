using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Permisos;

namespace AutoVentas.Services.Permisos;

/// <summary>
/// T04. Gestión de Perfiles de Usuario.
/// Construye, a partir de las filas planas de la tabla Permisos, el árbol de objetos
/// Composite (<see cref="PermisoComponente"/>) y resuelve qué permisos atómicos tiene
/// asignado un rol siguiendo (recursivamente) los permisos compuestos asignados a ese rol.
/// </summary>
public class ServicioPermisos
{
    private readonly RepositorioPermisos _repositorio = new();

    /// <summary>Arma el árbol completo de permisos (todas las raíces, es decir, los permisos
    /// sin padre) recorriendo recursivamente la relación IdPermisoPadre.</summary>
    public List<PermisoComponente> ObtenerArbolCompleto()
    {
        var filas = _repositorio.ObtenerTodos();
        var porId = filas.ToDictionary(f => f.IdPermiso);
        var nodos = new Dictionary<int, PermisoComponente>();

        PermisoComponente Construir(int idPermiso)
        {
            if (nodos.TryGetValue(idPermiso, out var existente)) return existente;

            var fila = porId[idPermiso];
            var tieneHijos = filas.Any(f => f.IdPermisoPadre == idPermiso);

            if (!tieneHijos)
            {
                var atomico = new PermisoAtomico(fila.IdPermiso, fila.Codigo, fila.Nombre);
                nodos[idPermiso] = atomico;
                return atomico;
            }

            var compuesto = new PermisoCompuesto(fila.IdPermiso, fila.Codigo, fila.Nombre);
            nodos[idPermiso] = compuesto; // se registra antes de recursar para evitar ciclos
            foreach (var hijo in filas.Where(f => f.IdPermisoPadre == idPermiso))
            {
                compuesto.Agregar(Construir(hijo.IdPermiso));
            }
            return compuesto;
        }

        return filas.Where(f => f.IdPermisoPadre is null)
                     .Select(f => Construir(f.IdPermiso))
                     .ToList();
    }

    /// <summary>Permisos atómicos que tiene efectivamente un rol (resolviendo los compuestos
    /// asignados de forma recursiva).</summary>
    public HashSet<string> ObtenerCodigosAtomicosDelRol(int idRol)
    {
        var codigosAsignados = _repositorio.ObtenerCodigosDelRol(idRol).ToHashSet();
        var arbol = ObtenerArbolCompleto();

        var resultado = new HashSet<string>();
        void Recorrer(PermisoComponente componente)
        {
            if (codigosAsignados.Contains(componente.Codigo))
            {
                foreach (var hoja in componente.ObtenerHojas())
                {
                    resultado.Add(hoja.Codigo);
                }
            }
            foreach (var hijo in componente.Hijos)
            {
                Recorrer(hijo);
            }
        }

        foreach (var raiz in arbol)
        {
            Recorrer(raiz);
        }

        return resultado;
    }

    public bool RolTienePermiso(int idRol, string codigoPermiso) =>
        ObtenerCodigosAtomicosDelRol(idRol).Contains(codigoPermiso);

    public void AsignarPermisoARol(int idRol, int idPermiso) => _repositorio.AsignarPermisoARol(idRol, idPermiso);

    public void QuitarPermisoDeRol(int idRol, int idPermiso) => _repositorio.QuitarPermisoDeRol(idRol, idPermiso);

    public HashSet<string> ObtenerCodigosDirectosDelRol(int idRol) => _repositorio.ObtenerCodigosDelRol(idRol).ToHashSet();
}
