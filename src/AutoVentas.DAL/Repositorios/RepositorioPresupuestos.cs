using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioPresupuestos : IRepositorio<Presupuesto>
{
    private static Presupuesto Mapear(SqlDataReader r) => new()
    {
        IdPresupuesto = r.GetInt32(r.GetOrdinal("IdPresupuesto")),
        IdVehiculo = r.GetInt32(r.GetOrdinal("IdVehiculo")),
        IdCliente = r.GetInt32(r.GetOrdinal("IdCliente")),
        IdUsuarioVendedor = r.GetInt32(r.GetOrdinal("IdUsuarioVendedor")),
        FechaPresupuesto = r.GetDateTime(r.GetOrdinal("FechaPresupuesto")),
        Monto = r.GetDecimal(r.GetOrdinal("Monto")),
        Estado = Enum.Parse<EstadoPresupuesto>(r.GetString(r.GetOrdinal("Estado"))),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador")),
        VehiculoDescripcion = r.GetString(r.GetOrdinal("Marca")) + " " + r.GetString(r.GetOrdinal("Modelo")),
        ClienteNombreCompleto = r.GetString(r.GetOrdinal("Apellido")) + ", " + r.GetString(r.GetOrdinal("Nombre"))
    };

    private const string SelectBase = @"
        SELECT p.IdPresupuesto, p.IdVehiculo, p.IdCliente, p.IdUsuarioVendedor, p.FechaPresupuesto,
               p.Monto, p.Estado, p.DigitoVerificador, v.Marca, v.Modelo, c.Nombre, c.Apellido
        FROM Presupuestos p
        INNER JOIN Vehiculos v ON v.IdVehiculo = p.IdVehiculo
        INNER JOIN Clientes c ON c.IdCliente = p.IdCliente";

    public int Agregar(Presupuesto p) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Presupuestos (IdVehiculo, IdCliente, IdUsuarioVendedor, FechaPresupuesto, Monto, Estado, DigitoVerificador)
          VALUES (@idVehiculo, @idCliente, @idVendedor, @fecha, @monto, @estado, @digito)",
        SqlHelper.Param("@idVehiculo", p.IdVehiculo),
        SqlHelper.Param("@idCliente", p.IdCliente),
        SqlHelper.Param("@idVendedor", p.IdUsuarioVendedor),
        SqlHelper.Param("@fecha", p.FechaPresupuesto),
        SqlHelper.Param("@monto", p.Monto),
        SqlHelper.Param("@estado", p.Estado.ToString()),
        SqlHelper.Param("@digito", p.DigitoVerificador));

    public void Modificar(Presupuesto p) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Presupuestos SET IdVehiculo=@idVehiculo, IdCliente=@idCliente, IdUsuarioVendedor=@idVendedor,
                 Monto=@monto, Estado=@estado, DigitoVerificador=@digito
          WHERE IdPresupuesto=@id",
        SqlHelper.Param("@idVehiculo", p.IdVehiculo),
        SqlHelper.Param("@idCliente", p.IdCliente),
        SqlHelper.Param("@idVendedor", p.IdUsuarioVendedor),
        SqlHelper.Param("@monto", p.Monto),
        SqlHelper.Param("@estado", p.Estado.ToString()),
        SqlHelper.Param("@digito", p.DigitoVerificador),
        SqlHelper.Param("@id", p.IdPresupuesto));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Presupuestos WHERE IdPresupuesto = @id", SqlHelper.Param("@id", id));

    public Presupuesto? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE p.IdPresupuesto = @id", Mapear, SqlHelper.Param("@id", id));

    public List<Presupuesto> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY p.FechaPresupuesto DESC", Mapear);
}
