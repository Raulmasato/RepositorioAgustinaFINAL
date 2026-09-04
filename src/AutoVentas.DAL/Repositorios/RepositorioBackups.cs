using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioBackups
{
    private static RegistroBackup Mapear(SqlDataReader r) => new()
    {
        IdBackup = r.GetInt32(r.GetOrdinal("IdBackup")),
        RutaArchivo = r.GetString(r.GetOrdinal("RutaArchivo")),
        FechaHora = r.GetDateTime(r.GetOrdinal("FechaHora")),
        IdUsuario = r.IsDBNull(r.GetOrdinal("IdUsuario")) ? null : r.GetInt32(r.GetOrdinal("IdUsuario")),
        Resultado = r.GetString(r.GetOrdinal("Resultado")),
        Detalle = r.IsDBNull(r.GetOrdinal("Detalle")) ? null : r.GetString(r.GetOrdinal("Detalle"))
    };

    public int Registrar(RegistroBackup registro) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Backups (RutaArchivo, IdUsuario, Resultado, Detalle)
          VALUES (@ruta, @idUsuario, @resultado, @detalle)",
        SqlHelper.Param("@ruta", registro.RutaArchivo),
        SqlHelper.Param("@idUsuario", registro.IdUsuario),
        SqlHelper.Param("@resultado", registro.Resultado),
        SqlHelper.Param("@detalle", registro.Detalle));

    public List<RegistroBackup> ObtenerCatalogo() => SqlHelper.EjecutarConsulta(
        "SELECT IdBackup, RutaArchivo, FechaHora, IdUsuario, Resultado, Detalle FROM Backups ORDER BY FechaHora DESC",
        Mapear);

    public RegistroBackup? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        "SELECT IdBackup, RutaArchivo, FechaHora, IdUsuario, Resultado, Detalle FROM Backups WHERE IdBackup = @id",
        Mapear, SqlHelper.Param("@id", id));
}
