using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioVehiculos : IRepositorio<Vehiculo>
{
    private static Vehiculo Mapear(SqlDataReader r) => new()
    {
        IdVehiculo = r.GetInt32(r.GetOrdinal("IdVehiculo")),
        Marca = r.GetString(r.GetOrdinal("Marca")),
        Modelo = r.GetString(r.GetOrdinal("Modelo")),
        Color = r.GetString(r.GetOrdinal("Color")),
        Anio = r.IsDBNull(r.GetOrdinal("Anio")) ? null : r.GetInt32(r.GetOrdinal("Anio")),
        Precio = r.IsDBNull(r.GetOrdinal("Precio")) ? null : r.GetDecimal(r.GetOrdinal("Precio")),
        Disponible = r.GetBoolean(r.GetOrdinal("Disponible")),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador"))
    };

    private const string SelectBase =
        "SELECT IdVehiculo, Marca, Modelo, Color, Anio, Precio, Disponible, DigitoVerificador FROM Vehiculos";

    public int Agregar(Vehiculo v) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Vehiculos (Marca, Modelo, Color, Anio, Precio, Disponible, DigitoVerificador)
          VALUES (@marca, @modelo, @color, @anio, @precio, @disponible, @digito)",
        SqlHelper.Param("@marca", v.Marca),
        SqlHelper.Param("@modelo", v.Modelo),
        SqlHelper.Param("@color", v.Color),
        SqlHelper.Param("@anio", v.Anio),
        SqlHelper.Param("@precio", v.Precio),
        SqlHelper.Param("@disponible", v.Disponible),
        SqlHelper.Param("@digito", v.DigitoVerificador));

    public void Modificar(Vehiculo v) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Vehiculos SET Marca=@marca, Modelo=@modelo, Color=@color, Anio=@anio,
                 Precio=@precio, Disponible=@disponible, DigitoVerificador=@digito
          WHERE IdVehiculo=@id",
        SqlHelper.Param("@marca", v.Marca),
        SqlHelper.Param("@modelo", v.Modelo),
        SqlHelper.Param("@color", v.Color),
        SqlHelper.Param("@anio", v.Anio),
        SqlHelper.Param("@precio", v.Precio),
        SqlHelper.Param("@disponible", v.Disponible),
        SqlHelper.Param("@digito", v.DigitoVerificador),
        SqlHelper.Param("@id", v.IdVehiculo));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Vehiculos WHERE IdVehiculo = @id", SqlHelper.Param("@id", id));

    public Vehiculo? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE IdVehiculo = @id", Mapear, SqlHelper.Param("@id", id));

    public List<Vehiculo> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY Marca, Modelo", Mapear);

    public List<Vehiculo> ObtenerDisponibles() => SqlHelper.EjecutarConsulta(
        SelectBase + " WHERE Disponible = 1 ORDER BY Marca, Modelo", Mapear);
}
