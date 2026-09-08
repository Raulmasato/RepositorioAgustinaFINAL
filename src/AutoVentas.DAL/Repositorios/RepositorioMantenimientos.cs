using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioMantenimientos : IRepositorio<Mantenimiento>
{
    private static Mantenimiento Mapear(SqlDataReader r) => new()
    {
        IdMantenimiento = r.GetInt32(r.GetOrdinal("IdMantenimiento")),
        IdVehiculo = r.GetInt32(r.GetOrdinal("IdVehiculo")),
        IdCliente = r.GetInt32(r.GetOrdinal("IdCliente")),
        Servicio = r.GetString(r.GetOrdinal("Servicio")),
        FechaServicio = r.GetDateTime(r.GetOrdinal("FechaServicio")),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador")),
        VehiculoDescripcion = r.GetString(r.GetOrdinal("Marca")) + " " + r.GetString(r.GetOrdinal("Modelo")),
        ClienteNombreCompleto = r.GetString(r.GetOrdinal("Apellido")) + ", " + r.GetString(r.GetOrdinal("Nombre"))
    };

    private const string SelectBase = @"
        SELECT m.IdMantenimiento, m.IdVehiculo, m.IdCliente, m.Servicio, m.FechaServicio, m.DigitoVerificador,
               v.Marca, v.Modelo, c.Nombre, c.Apellido
        FROM Mantenimientos m
        INNER JOIN Vehiculos v ON v.IdVehiculo = m.IdVehiculo
        INNER JOIN Clientes c ON c.IdCliente = m.IdCliente";

    public int Agregar(Mantenimiento m) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Mantenimientos (IdVehiculo, IdCliente, Servicio, FechaServicio, DigitoVerificador)
          VALUES (@idVehiculo, @idCliente, @servicio, @fecha, @digito)",
        SqlHelper.Param("@idVehiculo", m.IdVehiculo),
        SqlHelper.Param("@idCliente", m.IdCliente),
        SqlHelper.Param("@servicio", m.Servicio),
        SqlHelper.Param("@fecha", m.FechaServicio),
        SqlHelper.Param("@digito", m.DigitoVerificador));

    public void Modificar(Mantenimiento m) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Mantenimientos SET IdVehiculo=@idVehiculo, IdCliente=@idCliente, Servicio=@servicio,
                 FechaServicio=@fecha, DigitoVerificador=@digito
          WHERE IdMantenimiento=@id",
        SqlHelper.Param("@idVehiculo", m.IdVehiculo),
        SqlHelper.Param("@idCliente", m.IdCliente),
        SqlHelper.Param("@servicio", m.Servicio),
        SqlHelper.Param("@fecha", m.FechaServicio),
        SqlHelper.Param("@digito", m.DigitoVerificador),
        SqlHelper.Param("@id", m.IdMantenimiento));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Mantenimientos WHERE IdMantenimiento = @id", SqlHelper.Param("@id", id));

    public Mantenimiento? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE m.IdMantenimiento = @id", Mapear, SqlHelper.Param("@id", id));

    public List<Mantenimiento> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY m.FechaServicio DESC", Mapear);
}
