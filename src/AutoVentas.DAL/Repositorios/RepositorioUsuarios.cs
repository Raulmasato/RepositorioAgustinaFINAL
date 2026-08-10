using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioUsuarios : IRepositorio<Usuario>
{
    private static Usuario Mapear(SqlDataReader r) => new()
    {
        IdUsuario = r.GetInt32(r.GetOrdinal("IdUsuario")),
        NombreUsuario = r.GetString(r.GetOrdinal("NombreUsuario")),
        ClaveHash = r.GetString(r.GetOrdinal("ClaveHash")),
        ClaveSalt = r.GetString(r.GetOrdinal("ClaveSalt")),
        IdRol = r.GetInt32(r.GetOrdinal("IdRol")),
        Rol = (NombreRol)Enum.Parse(typeof(NombreRol), r.GetString(r.GetOrdinal("RolNombre"))),
        Activo = r.GetBoolean(r.GetOrdinal("Activo")),
        FechaCreacion = r.GetDateTime(r.GetOrdinal("FechaCreacion")),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador"))
    };

    private const string SelectBase = @"
        SELECT u.IdUsuario, u.NombreUsuario, u.ClaveHash, u.ClaveSalt, u.IdRol, r.Nombre AS RolNombre,
               u.Activo, u.FechaCreacion, u.DigitoVerificador
        FROM Usuarios u
        INNER JOIN Roles r ON r.IdRol = u.IdRol";

    public int Agregar(Usuario u) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Usuarios (NombreUsuario, ClaveHash, ClaveSalt, IdRol, Activo, DigitoVerificador)
          VALUES (@nombreUsuario, @claveHash, @claveSalt, @idRol, @activo, @digito)",
        SqlHelper.Param("@nombreUsuario", u.NombreUsuario),
        SqlHelper.Param("@claveHash", u.ClaveHash),
        SqlHelper.Param("@claveSalt", u.ClaveSalt),
        SqlHelper.Param("@idRol", u.IdRol),
        SqlHelper.Param("@activo", u.Activo),
        SqlHelper.Param("@digito", u.DigitoVerificador));

    public void Modificar(Usuario u) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Usuarios SET NombreUsuario=@nombreUsuario, ClaveHash=@claveHash, ClaveSalt=@claveSalt,
                 IdRol=@idRol, Activo=@activo, DigitoVerificador=@digito
          WHERE IdUsuario=@id",
        SqlHelper.Param("@nombreUsuario", u.NombreUsuario),
        SqlHelper.Param("@claveHash", u.ClaveHash),
        SqlHelper.Param("@claveSalt", u.ClaveSalt),
        SqlHelper.Param("@idRol", u.IdRol),
        SqlHelper.Param("@activo", u.Activo),
        SqlHelper.Param("@digito", u.DigitoVerificador),
        SqlHelper.Param("@id", u.IdUsuario));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "UPDATE Usuarios SET Activo = 0 WHERE IdUsuario = @id", SqlHelper.Param("@id", id));

    public Usuario? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE u.IdUsuario = @id", Mapear, SqlHelper.Param("@id", id));

    public Usuario? ObtenerPorNombreUsuario(string nombreUsuario) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE u.NombreUsuario = @nombreUsuario", Mapear,
        SqlHelper.Param("@nombreUsuario", nombreUsuario));

    public List<Usuario> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY u.NombreUsuario", Mapear);

    public bool ExisteNombreUsuario(string nombreUsuario) =>
        Convert.ToInt32(SqlHelper.EjecutarScalar(
            "SELECT COUNT(1) FROM Usuarios WHERE NombreUsuario = @nombreUsuario",
            SqlHelper.Param("@nombreUsuario", nombreUsuario))) > 0;
}
