using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioControlCambios
{
    private static RegistroControlCambio Mapear(SqlDataReader r) => new()
    {
        IdControlCambio = r.GetInt64(r.GetOrdinal("IdControlCambio")),
        Tabla = r.GetString(r.GetOrdinal("Tabla")),
        IdRegistro = r.GetInt32(r.GetOrdinal("IdRegistro")),
        Campo = r.GetString(r.GetOrdinal("Campo")),
        ValorAnterior = r.IsDBNull(r.GetOrdinal("ValorAnterior")) ? null : r.GetString(r.GetOrdinal("ValorAnterior")),
        ValorNuevo = r.IsDBNull(r.GetOrdinal("ValorNuevo")) ? null : r.GetString(r.GetOrdinal("ValorNuevo")),
        TipoOperacion = r.GetString(r.GetOrdinal("TipoOperacion")),
        IdUsuario = r.IsDBNull(r.GetOrdinal("IdUsuario")) ? null : r.GetInt32(r.GetOrdinal("IdUsuario")),
        FechaHora = r.GetDateTime(r.GetOrdinal("FechaHora")),
        NombreUsuario = r.IsDBNull(r.GetOrdinal("NombreUsuario")) ? null : r.GetString(r.GetOrdinal("NombreUsuario"))
    };

    public void Registrar(RegistroControlCambio registro) => SqlHelper.EjecutarNonQuery(
        @"INSERT INTO ControlCambios (Tabla, IdRegistro, Campo, ValorAnterior, ValorNuevo, TipoOperacion, IdUsuario)
          VALUES (@tabla, @idRegistro, @campo, @anterior, @nuevo, @tipo, @idUsuario)",
        SqlHelper.Param("@tabla", registro.Tabla),
        SqlHelper.Param("@idRegistro", registro.IdRegistro),
        SqlHelper.Param("@campo", registro.Campo),
        SqlHelper.Param("@anterior", registro.ValorAnterior),
        SqlHelper.Param("@nuevo", registro.ValorNuevo),
        SqlHelper.Param("@tipo", registro.TipoOperacion),
        SqlHelper.Param("@idUsuario", registro.IdUsuario));

    /// <summary>Historial completo de una entidad puntual, para poder reconstruir su estado anterior.</summary>
    public List<RegistroControlCambio> ObtenerHistorial(string tabla, int idRegistro) => SqlHelper.EjecutarConsulta(
        @"SELECT cc.IdControlCambio, cc.Tabla, cc.IdRegistro, cc.Campo, cc.ValorAnterior, cc.ValorNuevo,
                 cc.TipoOperacion, cc.IdUsuario, cc.FechaHora, u.NombreUsuario
          FROM ControlCambios cc
          LEFT JOIN Usuarios u ON u.IdUsuario = cc.IdUsuario
          WHERE cc.Tabla = @tabla AND cc.IdRegistro = @idRegistro
          ORDER BY cc.FechaHora DESC",
        Mapear,
        SqlHelper.Param("@tabla", tabla),
        SqlHelper.Param("@idRegistro", idRegistro));
}
