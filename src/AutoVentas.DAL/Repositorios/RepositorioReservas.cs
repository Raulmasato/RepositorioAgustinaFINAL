using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioReservas : IRepositorio<Reserva>
{
    private static Reserva Mapear(SqlDataReader r) => new()
    {
        IdReserva = r.GetInt32(r.GetOrdinal("IdReserva")),
        IdVehiculo = r.GetInt32(r.GetOrdinal("IdVehiculo")),
        IdCliente = r.GetInt32(r.GetOrdinal("IdCliente")),
        IdUsuarioEjecutivo = r.IsDBNull(r.GetOrdinal("IdUsuarioEjecutivo")) ? null : r.GetInt32(r.GetOrdinal("IdUsuarioEjecutivo")),
        FechaReserva = r.GetDateTime(r.GetOrdinal("FechaReserva")),
        FechaVencimiento = r.GetDateTime(r.GetOrdinal("FechaVencimiento")),
        Estado = Enum.Parse<EstadoReserva>(r.GetString(r.GetOrdinal("Estado"))),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador")),
        VehiculoDescripcion = r.GetString(r.GetOrdinal("Marca")) + " " + r.GetString(r.GetOrdinal("Modelo")),
        ClienteNombreCompleto = r.GetString(r.GetOrdinal("Apellido")) + ", " + r.GetString(r.GetOrdinal("Nombre"))
    };

    private const string SelectBase = @"
        SELECT re.IdReserva, re.IdVehiculo, re.IdCliente, re.IdUsuarioEjecutivo, re.FechaReserva,
               re.FechaVencimiento, re.Estado, re.DigitoVerificador, v.Marca, v.Modelo, c.Nombre, c.Apellido
        FROM Reservas re
        INNER JOIN Vehiculos v ON v.IdVehiculo = re.IdVehiculo
        INNER JOIN Clientes c ON c.IdCliente = re.IdCliente";

    public int Agregar(Reserva r) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Reservas (IdVehiculo, IdCliente, IdUsuarioEjecutivo, FechaReserva, FechaVencimiento, Estado, DigitoVerificador)
          VALUES (@idVehiculo, @idCliente, @idEjecutivo, @fechaReserva, @fechaVencimiento, @estado, @digito)",
        SqlHelper.Param("@idVehiculo", r.IdVehiculo),
        SqlHelper.Param("@idCliente", r.IdCliente),
        SqlHelper.Param("@idEjecutivo", r.IdUsuarioEjecutivo),
        SqlHelper.Param("@fechaReserva", r.FechaReserva),
        SqlHelper.Param("@fechaVencimiento", r.FechaVencimiento),
        SqlHelper.Param("@estado", r.Estado.ToString()),
        SqlHelper.Param("@digito", r.DigitoVerificador));

    public void Modificar(Reserva r) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Reservas SET IdVehiculo=@idVehiculo, IdCliente=@idCliente, IdUsuarioEjecutivo=@idEjecutivo,
                 FechaVencimiento=@fechaVencimiento, Estado=@estado, DigitoVerificador=@digito
          WHERE IdReserva=@id",
        SqlHelper.Param("@idVehiculo", r.IdVehiculo),
        SqlHelper.Param("@idCliente", r.IdCliente),
        SqlHelper.Param("@idEjecutivo", r.IdUsuarioEjecutivo),
        SqlHelper.Param("@fechaVencimiento", r.FechaVencimiento),
        SqlHelper.Param("@estado", r.Estado.ToString()),
        SqlHelper.Param("@digito", r.DigitoVerificador),
        SqlHelper.Param("@id", r.IdReserva));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Reservas WHERE IdReserva = @id", SqlHelper.Param("@id", id));

    public Reserva? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE re.IdReserva = @id", Mapear, SqlHelper.Param("@id", id));

    public List<Reserva> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY re.FechaReserva DESC", Mapear);

    public List<Reserva> ObtenerPorCliente(int idCliente) => SqlHelper.EjecutarConsulta(
        SelectBase + " WHERE re.IdCliente = @idCliente ORDER BY re.FechaReserva DESC", Mapear,
        SqlHelper.Param("@idCliente", idCliente));
}
