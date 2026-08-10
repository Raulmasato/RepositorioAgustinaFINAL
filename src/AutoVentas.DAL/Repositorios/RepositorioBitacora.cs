using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioBitacora
{
    private static RegistroBitacora Mapear(SqlDataReader r) => new()
    {
        IdBitacora = r.GetInt64(r.GetOrdinal("IdBitacora")),
        FechaHora = r.GetDateTime(r.GetOrdinal("FechaHora")),
        IdUsuario = r.IsDBNull(r.GetOrdinal("IdUsuario")) ? null : r.GetInt32(r.GetOrdinal("IdUsuario")),
        Actividad = r.GetString(r.GetOrdinal("Actividad")),
        Informacion = r.IsDBNull(r.GetOrdinal("Informacion")) ? null : r.GetString(r.GetOrdinal("Informacion")),
        NombreUsuario = r.IsDBNull(r.GetOrdinal("NombreUsuario")) ? null : r.GetString(r.GetOrdinal("NombreUsuario"))
    };

    public void Registrar(int? idUsuario, string actividad, string? informacion) => SqlHelper.EjecutarNonQuery(
        "INSERT INTO Bitacora (IdUsuario, Actividad, Informacion) VALUES (@idUsuario, @actividad, @informacion)",
        SqlHelper.Param("@idUsuario", idUsuario),
        SqlHelper.Param("@actividad", actividad),
        SqlHelper.Param("@informacion", informacion));

    /// <summary>Búsqueda combinada por usuario, actividad y rango de fechas (todos opcionales).</summary>
    public List<RegistroBitacora> Buscar(int? idUsuario, string? actividad, DateTime? desde, DateTime? hasta)
    {
        var sql = @"SELECT b.IdBitacora, b.FechaHora, b.IdUsuario, b.Actividad, b.Informacion, u.NombreUsuario
                     FROM Bitacora b
                     LEFT JOIN Usuarios u ON u.IdUsuario = b.IdUsuario
                     WHERE (@idUsuario IS NULL OR b.IdUsuario = @idUsuario)
                       AND (@actividad IS NULL OR b.Actividad LIKE '%' + @actividad + '%')
                       AND (@desde IS NULL OR b.FechaHora >= @desde)
                       AND (@hasta IS NULL OR b.FechaHora <= @hasta)
                     ORDER BY b.FechaHora DESC";

        return SqlHelper.EjecutarConsulta(sql, Mapear,
            SqlHelper.Param("@idUsuario", idUsuario),
            SqlHelper.Param("@actividad", actividad),
            SqlHelper.Param("@desde", desde),
            SqlHelper.Param("@hasta", hasta));
    }
}
