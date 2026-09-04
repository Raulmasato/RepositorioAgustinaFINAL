using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioReportes : IRepositorio<Reporte>
{
    private static Reporte Mapear(SqlDataReader r) => new()
    {
        IdReporte = r.GetInt32(r.GetOrdinal("IdReporte")),
        Titulo = r.GetString(r.GetOrdinal("Titulo")),
        TipoReporte = Enum.Parse<TipoReporte>(r.GetString(r.GetOrdinal("TipoReporte"))),
        FechaDesde = r.GetDateTime(r.GetOrdinal("FechaDesde")),
        FechaHasta = r.GetDateTime(r.GetOrdinal("FechaHasta")),
        Contenido = r.IsDBNull(r.GetOrdinal("Contenido")) ? null : r.GetString(r.GetOrdinal("Contenido")),
        IdUsuarioEjecutivo = r.GetInt32(r.GetOrdinal("IdUsuarioEjecutivo")),
        FechaGeneracion = r.GetDateTime(r.GetOrdinal("FechaGeneracion")),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador")),
        PorcentajeCantidad = r.IsDBNull(r.GetOrdinal("PorcentajeCantidad")) ? null : r.GetDecimal(r.GetOrdinal("PorcentajeCantidad")),
        PorcentajeMonto = r.IsDBNull(r.GetOrdinal("PorcentajeMonto")) ? null : r.GetDecimal(r.GetOrdinal("PorcentajeMonto"))
    };

    private const string SelectBase = @"
        SELECT IdReporte, Titulo, TipoReporte, FechaDesde, FechaHasta, Contenido, IdUsuarioEjecutivo,
               FechaGeneracion, DigitoVerificador, PorcentajeCantidad, PorcentajeMonto
        FROM Reportes";

    public int Agregar(Reporte r) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Reportes (Titulo, TipoReporte, FechaDesde, FechaHasta, Contenido, IdUsuarioEjecutivo, DigitoVerificador, PorcentajeCantidad, PorcentajeMonto)
          VALUES (@titulo, @tipo, @desde, @hasta, @contenido, @idEjecutivo, @digito, @porcCantidad, @porcMonto)",
        SqlHelper.Param("@titulo", r.Titulo),
        SqlHelper.Param("@tipo", r.TipoReporte.ToString()),
        SqlHelper.Param("@desde", r.FechaDesde),
        SqlHelper.Param("@hasta", r.FechaHasta),
        SqlHelper.Param("@contenido", r.Contenido),
        SqlHelper.Param("@idEjecutivo", r.IdUsuarioEjecutivo),
        SqlHelper.Param("@digito", r.DigitoVerificador),
        SqlHelper.Param("@porcCantidad", r.PorcentajeCantidad),
        SqlHelper.Param("@porcMonto", r.PorcentajeMonto));

    public void Modificar(Reporte r) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Reportes SET Titulo=@titulo, TipoReporte=@tipo, FechaDesde=@desde, FechaHasta=@hasta,
                 Contenido=@contenido, DigitoVerificador=@digito, PorcentajeCantidad=@porcCantidad, PorcentajeMonto=@porcMonto
          WHERE IdReporte=@id",
        SqlHelper.Param("@titulo", r.Titulo),
        SqlHelper.Param("@tipo", r.TipoReporte.ToString()),
        SqlHelper.Param("@desde", r.FechaDesde),
        SqlHelper.Param("@hasta", r.FechaHasta),
        SqlHelper.Param("@contenido", r.Contenido),
        SqlHelper.Param("@digito", r.DigitoVerificador),
        SqlHelper.Param("@porcCantidad", r.PorcentajeCantidad),
        SqlHelper.Param("@porcMonto", r.PorcentajeMonto),
        SqlHelper.Param("@id", r.IdReporte));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Reportes WHERE IdReporte = @id", SqlHelper.Param("@id", id));

    public Reporte? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE IdReporte = @id", Mapear, SqlHelper.Param("@id", id));

    public List<Reporte> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY FechaGeneracion DESC", Mapear);
}
