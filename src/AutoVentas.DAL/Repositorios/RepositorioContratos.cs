using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioContratos : IRepositorio<Contrato>
{
    private static Contrato Mapear(SqlDataReader r) => new()
    {
        IdContrato = r.GetInt32(r.GetOrdinal("IdContrato")),
        IdVehiculo = r.GetInt32(r.GetOrdinal("IdVehiculo")),
        IdCliente = r.GetInt32(r.GetOrdinal("IdCliente")),
        IdUsuarioEjecutivo = r.GetInt32(r.GetOrdinal("IdUsuarioEjecutivo")),
        IdPresupuesto = r.IsDBNull(r.GetOrdinal("IdPresupuesto")) ? null : r.GetInt32(r.GetOrdinal("IdPresupuesto")),
        FechaContrato = r.GetDateTime(r.GetOrdinal("FechaContrato")),
        Precio = r.GetDecimal(r.GetOrdinal("Precio")),
        Estado = Enum.Parse<EstadoContrato>(r.GetString(r.GetOrdinal("Estado"))),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador")),
        VehiculoDescripcion = r.GetString(r.GetOrdinal("Marca")) + " " + r.GetString(r.GetOrdinal("Modelo")),
        ClienteNombreCompleto = r.GetString(r.GetOrdinal("Apellido")) + ", " + r.GetString(r.GetOrdinal("Nombre"))
    };

    private const string SelectBase = @"
        SELECT co.IdContrato, co.IdVehiculo, co.IdCliente, co.IdUsuarioEjecutivo, co.IdPresupuesto,
               co.FechaContrato, co.Precio, co.Estado, co.DigitoVerificador, v.Marca, v.Modelo, c.Nombre, c.Apellido
        FROM Contratos co
        INNER JOIN Vehiculos v ON v.IdVehiculo = co.IdVehiculo
        INNER JOIN Clientes c ON c.IdCliente = co.IdCliente";

    public int Agregar(Contrato c) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Contratos (IdVehiculo, IdCliente, IdUsuarioEjecutivo, IdPresupuesto, FechaContrato, Precio, Estado, DigitoVerificador)
          VALUES (@idVehiculo, @idCliente, @idEjecutivo, @idPresupuesto, @fecha, @precio, @estado, @digito)",
        SqlHelper.Param("@idVehiculo", c.IdVehiculo),
        SqlHelper.Param("@idCliente", c.IdCliente),
        SqlHelper.Param("@idEjecutivo", c.IdUsuarioEjecutivo),
        SqlHelper.Param("@idPresupuesto", c.IdPresupuesto),
        SqlHelper.Param("@fecha", c.FechaContrato),
        SqlHelper.Param("@precio", c.Precio),
        SqlHelper.Param("@estado", c.Estado.ToString()),
        SqlHelper.Param("@digito", c.DigitoVerificador));

    public void Modificar(Contrato c) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Contratos SET IdVehiculo=@idVehiculo, IdCliente=@idCliente, IdUsuarioEjecutivo=@idEjecutivo,
                 IdPresupuesto=@idPresupuesto, Precio=@precio, Estado=@estado, DigitoVerificador=@digito
          WHERE IdContrato=@id",
        SqlHelper.Param("@idVehiculo", c.IdVehiculo),
        SqlHelper.Param("@idCliente", c.IdCliente),
        SqlHelper.Param("@idEjecutivo", c.IdUsuarioEjecutivo),
        SqlHelper.Param("@idPresupuesto", c.IdPresupuesto),
        SqlHelper.Param("@precio", c.Precio),
        SqlHelper.Param("@estado", c.Estado.ToString()),
        SqlHelper.Param("@digito", c.DigitoVerificador),
        SqlHelper.Param("@id", c.IdContrato));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Contratos WHERE IdContrato = @id", SqlHelper.Param("@id", id));

    public Contrato? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE co.IdContrato = @id", Mapear, SqlHelper.Param("@id", id));

    public List<Contrato> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY co.FechaContrato DESC", Mapear);
}
