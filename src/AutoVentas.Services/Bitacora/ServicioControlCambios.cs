using System.Reflection;
using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.Services.Bitacora;

/// <summary>
/// T06b. Control de cambios (auditoría): registra, campo a campo, quién, cuándo y qué cambió
/// en una entidad, de forma que se pueda reconstruir el estado anterior de un objeto.
/// Usa reflexión sobre las propiedades públicas de la entidad para no acoplar este servicio
/// a cada clase de negocio en particular (reuso entre todas las gestiones CRUD del sistema).
/// </summary>
public class ServicioControlCambios
{
    private readonly RepositorioControlCambios _repositorio = new();

    public void RegistrarAlta<T>(string tabla, int idRegistro, T entidad)
    {
        var idUsuario = SesionActual.Instancia.UsuarioLogueado?.IdUsuario;
        foreach (var propiedad in ObtenerPropiedades<T>())
        {
            _repositorio.Registrar(new RegistroControlCambio
            {
                Tabla = tabla,
                IdRegistro = idRegistro,
                Campo = propiedad.Name,
                ValorAnterior = null,
                ValorNuevo = ObtenerValorTexto(propiedad, entidad),
                TipoOperacion = "INSERT",
                IdUsuario = idUsuario
            });
        }
    }

    public void RegistrarModificacion<T>(string tabla, int idRegistro, T entidadAnterior, T entidadNueva)
    {
        var idUsuario = SesionActual.Instancia.UsuarioLogueado?.IdUsuario;
        foreach (var propiedad in ObtenerPropiedades<T>())
        {
            var valorAnterior = ObtenerValorTexto(propiedad, entidadAnterior);
            var valorNuevo = ObtenerValorTexto(propiedad, entidadNueva);

            if (valorAnterior == valorNuevo) continue;

            _repositorio.Registrar(new RegistroControlCambio
            {
                Tabla = tabla,
                IdRegistro = idRegistro,
                Campo = propiedad.Name,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                TipoOperacion = "UPDATE",
                IdUsuario = idUsuario
            });
        }
    }

    public void RegistrarBaja<T>(string tabla, int idRegistro, T entidad)
    {
        var idUsuario = SesionActual.Instancia.UsuarioLogueado?.IdUsuario;
        _repositorio.Registrar(new RegistroControlCambio
        {
            Tabla = tabla,
            IdRegistro = idRegistro,
            Campo = "*",
            ValorAnterior = ResumirEntidad(entidad),
            ValorNuevo = null,
            TipoOperacion = "DELETE",
            IdUsuario = idUsuario
        });
    }

    public List<RegistroControlCambio> ObtenerHistorial(string tabla, int idRegistro)
        => _repositorio.ObtenerHistorial(tabla, idRegistro);

    private static IEnumerable<PropertyInfo> ObtenerPropiedades<T>() =>
        typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0);

    private static string? ObtenerValorTexto<T>(PropertyInfo propiedad, T entidad)
    {
        var valor = propiedad.GetValue(entidad);
        return valor?.ToString();
    }

    private static string ResumirEntidad<T>(T entidad) =>
        string.Join("; ", ObtenerPropiedades<T>().Select(p => $"{p.Name}={ObtenerValorTexto(p, entidad)}"));
}
