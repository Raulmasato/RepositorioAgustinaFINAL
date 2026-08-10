using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.Services.Bitacora;

/// <summary>T06a. Gestión de Bitácora: registra fecha, hora, usuario, actividad e información
/// asociada de cada operación relevante realizada en el sistema.</summary>
public class ServicioBitacora
{
    private readonly RepositorioBitacora _repositorio = new();

    public void Registrar(string actividad, string? informacion = null)
    {
        var idUsuario = SesionActual.Instancia.UsuarioLogueado?.IdUsuario;
        _repositorio.Registrar(idUsuario, actividad, informacion);
    }

    public List<RegistroBitacora> Buscar(int? idUsuario, string? actividad, DateTime? desde, DateTime? hasta)
        => _repositorio.Buscar(idUsuario, actividad, desde, hasta);
}
