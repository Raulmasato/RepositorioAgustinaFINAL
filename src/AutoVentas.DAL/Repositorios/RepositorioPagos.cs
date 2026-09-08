using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioPagos : IRepositorio<Pago>
{
    private static Pago Mapear(SqlDataReader r) => new()
    {
        IdPago = r.GetInt32(r.GetOrdinal("IdPago")),
        IdContrato = r.GetInt32(r.GetOrdinal("IdContrato")),
        IdUsuarioEjecutivo = r.GetInt32(r.GetOrdinal("IdUsuarioEjecutivo")),
        Monto = r.GetDecimal(r.GetOrdinal("Monto")),
        FechaPago = r.GetDateTime(r.GetOrdinal("FechaPago")),
        MetodoPago = r.GetString(r.GetOrdinal("MetodoPago")),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador")),
        ContratoDescripcion = $"Contrato #{r.GetInt32(r.GetOrdinal("IdContrato"))} - {r.GetString(r.GetOrdinal("Marca"))} {r.GetString(r.GetOrdinal("Modelo"))}"
    };

    private const string SelectBase = @"
        SELECT pa.IdPago, pa.IdContrato, pa.IdUsuarioEjecutivo, pa.Monto, pa.FechaPago, pa.MetodoPago, pa.DigitoVerificador,
               v.Marca, v.Modelo
        FROM Pagos pa
        INNER JOIN Contratos co ON co.IdContrato = pa.IdContrato
        INNER JOIN Vehiculos v ON v.IdVehiculo = co.IdVehiculo";

    public int Agregar(Pago p) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Pagos (IdContrato, IdUsuarioEjecutivo, Monto, FechaPago, MetodoPago, DigitoVerificador)
          VALUES (@idContrato, @idEjecutivo, @monto, @fecha, @metodo, @digito)",
        SqlHelper.Param("@idContrato", p.IdContrato),
        SqlHelper.Param("@idEjecutivo", p.IdUsuarioEjecutivo),
        SqlHelper.Param("@monto", p.Monto),
        SqlHelper.Param("@fecha", p.FechaPago),
        SqlHelper.Param("@metodo", p.MetodoPago),
        SqlHelper.Param("@digito", p.DigitoVerificador));

    public void Modificar(Pago p) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Pagos SET IdContrato=@idContrato, IdUsuarioEjecutivo=@idEjecutivo, Monto=@monto,
                 MetodoPago=@metodo, DigitoVerificador=@digito
          WHERE IdPago=@id",
        SqlHelper.Param("@idContrato", p.IdContrato),
        SqlHelper.Param("@idEjecutivo", p.IdUsuarioEjecutivo),
        SqlHelper.Param("@monto", p.Monto),
        SqlHelper.Param("@metodo", p.MetodoPago),
        SqlHelper.Param("@digito", p.DigitoVerificador),
        SqlHelper.Param("@id", p.IdPago));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Pagos WHERE IdPago = @id", SqlHelper.Param("@id", id));

    public Pago? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE pa.IdPago = @id", Mapear, SqlHelper.Param("@id", id));

    public List<Pago> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY pa.FechaPago DESC", Mapear);
}
