using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioEntregas : IRepositorio<Entrega>
{
    private static Entrega Mapear(SqlDataReader r) => new()
    {
        IdEntrega = r.GetInt32(r.GetOrdinal("IdEntrega")),
        IdContrato = r.GetInt32(r.GetOrdinal("IdContrato")),
        IdUsuarioEjecutivo = r.GetInt32(r.GetOrdinal("IdUsuarioEjecutivo")),
        FechaEntrega = r.GetDateTime(r.GetOrdinal("FechaEntrega")),
        LugarEntrega = r.GetString(r.GetOrdinal("LugarEntrega")),
        Estado = Enum.Parse<EstadoEntrega>(r.GetString(r.GetOrdinal("Estado"))),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador")),
        ContratoDescripcion = $"Contrato #{r.GetInt32(r.GetOrdinal("IdContrato"))} - {r.GetString(r.GetOrdinal("Marca"))} {r.GetString(r.GetOrdinal("Modelo"))}"
    };

    private const string SelectBase = @"
        SELECT e.IdEntrega, e.IdContrato, e.IdUsuarioEjecutivo, e.FechaEntrega, e.LugarEntrega, e.Estado, e.DigitoVerificador,
               v.Marca, v.Modelo
        FROM Entregas e
        INNER JOIN Contratos co ON co.IdContrato = e.IdContrato
        INNER JOIN Vehiculos v ON v.IdVehiculo = co.IdVehiculo";

    public int Agregar(Entrega e) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Entregas (IdContrato, IdUsuarioEjecutivo, FechaEntrega, LugarEntrega, Estado, DigitoVerificador)
          VALUES (@idContrato, @idEjecutivo, @fecha, @lugar, @estado, @digito)",
        SqlHelper.Param("@idContrato", e.IdContrato),
        SqlHelper.Param("@idEjecutivo", e.IdUsuarioEjecutivo),
        SqlHelper.Param("@fecha", e.FechaEntrega),
        SqlHelper.Param("@lugar", e.LugarEntrega),
        SqlHelper.Param("@estado", e.Estado.ToString()),
        SqlHelper.Param("@digito", e.DigitoVerificador));

    public void Modificar(Entrega e) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Entregas SET IdContrato=@idContrato, IdUsuarioEjecutivo=@idEjecutivo, FechaEntrega=@fecha,
                 LugarEntrega=@lugar, Estado=@estado, DigitoVerificador=@digito
          WHERE IdEntrega=@id",
        SqlHelper.Param("@idContrato", e.IdContrato),
        SqlHelper.Param("@idEjecutivo", e.IdUsuarioEjecutivo),
        SqlHelper.Param("@fecha", e.FechaEntrega),
        SqlHelper.Param("@lugar", e.LugarEntrega),
        SqlHelper.Param("@estado", e.Estado.ToString()),
        SqlHelper.Param("@digito", e.DigitoVerificador),
        SqlHelper.Param("@id", e.IdEntrega));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Entregas WHERE IdEntrega = @id", SqlHelper.Param("@id", id));

    public Entrega? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE e.IdEntrega = @id", Mapear, SqlHelper.Param("@id", id));

    public List<Entrega> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY e.FechaEntrega DESC", Mapear);
}
