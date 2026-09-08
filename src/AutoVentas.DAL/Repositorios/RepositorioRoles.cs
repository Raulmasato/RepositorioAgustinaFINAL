using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioRoles
{
    private static Rol Mapear(SqlDataReader r) => new()
    {
        IdRol = r.GetInt32(r.GetOrdinal("IdRol")),
        Nombre = r.GetString(r.GetOrdinal("Nombre"))
    };

    public List<Rol> ObtenerTodos() =>
        SqlHelper.EjecutarConsulta("SELECT IdRol, Nombre FROM Roles ORDER BY Nombre", Mapear);

    public Rol? ObtenerPorId(int id) =>
        SqlHelper.EjecutarConsultaUno("SELECT IdRol, Nombre FROM Roles WHERE IdRol = @id", Mapear,
            SqlHelper.Param("@id", id));

    public Rol? ObtenerPorNombre(string nombre) =>
        SqlHelper.EjecutarConsultaUno("SELECT IdRol, Nombre FROM Roles WHERE Nombre = @nombre", Mapear,
            SqlHelper.Param("@nombre", nombre));
}
