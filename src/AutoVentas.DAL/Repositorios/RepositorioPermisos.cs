using AutoVentas.DAL.Conexion;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

/// <summary>Fila cruda de la tabla Permisos, usada por Services para construir el árbol Composite.</summary>
public record FilaPermiso(int IdPermiso, string Codigo, string Nombre, int? IdPermisoPadre);

public class RepositorioPermisos
{
    public List<FilaPermiso> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        "SELECT IdPermiso, Codigo, Nombre, IdPermisoPadre FROM Permisos ORDER BY IdPermiso",
        r => new FilaPermiso(
            r.GetInt32(r.GetOrdinal("IdPermiso")),
            r.GetString(r.GetOrdinal("Codigo")),
            r.GetString(r.GetOrdinal("Nombre")),
            r.IsDBNull(r.GetOrdinal("IdPermisoPadre")) ? null : r.GetInt32(r.GetOrdinal("IdPermisoPadre"))));

    /// <summary>Códigos de permiso asignados directamente a un rol (la raíz de su perfil).</summary>
    public List<string> ObtenerCodigosDelRol(int idRol) => SqlHelper.EjecutarConsulta(
        @"SELECT p.Codigo FROM RolPermisos rp
          INNER JOIN Permisos p ON p.IdPermiso = rp.IdPermiso
          WHERE rp.IdRol = @idRol",
        r => r.GetString(r.GetOrdinal("Codigo")),
        SqlHelper.Param("@idRol", idRol));

    public void AsignarPermisoARol(int idRol, int idPermiso)
    {
        if (Convert.ToInt32(SqlHelper.EjecutarScalar(
                "SELECT COUNT(1) FROM RolPermisos WHERE IdRol=@idRol AND IdPermiso=@idPermiso",
                SqlHelper.Param("@idRol", idRol), SqlHelper.Param("@idPermiso", idPermiso))) > 0)
        {
            return;
        }

        SqlHelper.EjecutarNonQuery(
            "INSERT INTO RolPermisos (IdRol, IdPermiso) VALUES (@idRol, @idPermiso)",
            SqlHelper.Param("@idRol", idRol), SqlHelper.Param("@idPermiso", idPermiso));
    }

    public void QuitarPermisoDeRol(int idRol, int idPermiso) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM RolPermisos WHERE IdRol=@idRol AND IdPermiso=@idPermiso",
        SqlHelper.Param("@idRol", idRol), SqlHelper.Param("@idPermiso", idPermiso));
}
