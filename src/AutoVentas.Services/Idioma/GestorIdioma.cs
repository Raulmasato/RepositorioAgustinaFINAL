using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;

namespace AutoVentas.Services.Idioma;

/// <summary>
/// T05. Gestión de Múltiples Idiomas — patrón Observer + Singleton.
/// Las traducciones se leen dinámicamente desde la base de datos (tablas Idiomas/Traducciones),
/// nunca desde archivos de recursos (.resx) estáticos, de forma que se puedan agregar nuevos
/// idiomas y leyendas en caliente sin recompilar. Cuando el idioma cambia, se notifica a todos
/// los formularios suscriptos para que actualicen sus textos sin recargarse.
/// </summary>
public sealed class GestorIdioma
{
    private static readonly Lazy<GestorIdioma> InstanciaPerezosa = new(() => new GestorIdioma());
    public static GestorIdioma Instancia => InstanciaPerezosa.Value;

    private readonly RepositorioIdiomas _repositorioIdiomas = new();
    private readonly List<IObservadorIdioma> _observadores = new();
    private Dictionary<string, string> _traducciones = new();

    private GestorIdioma() { }

    public string CodigoIdiomaActual { get; private set; } = "es";
    public List<Idioma> IdiomasDisponibles { get; private set; } = new();

    public void Inicializar(string codigoIdiomaPorDefecto = "es")
    {
        IdiomasDisponibles = _repositorioIdiomas.ObtenerTodos();
        CambiarIdioma(codigoIdiomaPorDefecto);
    }

    public void Suscribir(IObservadorIdioma observador)
    {
        if (!_observadores.Contains(observador))
        {
            _observadores.Add(observador);
        }
    }

    public void Desuscribir(IObservadorIdioma observador) => _observadores.Remove(observador);

    public void CambiarIdioma(string codigoIdioma)
    {
        _traducciones = _repositorioIdiomas.ObtenerTraducciones(codigoIdioma);
        CodigoIdiomaActual = codigoIdioma;
        NotificarObservadores();
    }

    /// <summary>Traduce una clave; si no existe traducción, devuelve la clave misma como fallback visible.</summary>
    public string Traducir(string clave) => _traducciones.TryGetValue(clave, out var valor) ? valor : clave;

    private void NotificarObservadores()
    {
        // ToList() evita problemas si algún observador se desuscribe durante la notificación.
        foreach (var observador in _observadores.ToList())
        {
            observador.ActualizarIdioma();
        }
    }
}
