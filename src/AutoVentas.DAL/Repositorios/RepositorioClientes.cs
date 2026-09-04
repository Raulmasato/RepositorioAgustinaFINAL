using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioClientes : IRepositorio<Cliente>
{
    private static Cliente Mapear(SqlDataReader r) => new()
    {
        IdCliente = r.GetInt32(r.GetOrdinal("IdCliente")),
        Nombre = r.GetString(r.GetOrdinal("Nombre")),
        Apellido = r.GetString(r.GetOrdinal("Apellido")),
        DniEncriptado = r.GetString(r.GetOrdinal("DniEncriptado")),
        IdUsuario = r.IsDBNull(r.GetOrdinal("IdUsuario")) ? null : r.GetInt32(r.GetOrdinal("IdUsuario")),
        DigitoVerificador = r.IsDBNull(r.GetOrdinal("DigitoVerificador")) ? null : r.GetString(r.GetOrdinal("DigitoVerificador"))
    };

    private const string SelectBase =
        "SELECT IdCliente, Nombre, Apellido, DniEncriptado, IdUsuario, DigitoVerificador FROM Clientes";

    public int Agregar(Cliente c) => SqlHelper.EjecutarInsertYObtenerId(
        @"INSERT INTO Clientes (Nombre, Apellido, DniEncriptado, IdUsuario, DigitoVerificador)
          VALUES (@nombre, @apellido, @dni, @idUsuario, @digito)",
        SqlHelper.Param("@nombre", c.Nombre),
        SqlHelper.Param("@apellido", c.Apellido),
        SqlHelper.Param("@dni", c.DniEncriptado),
        SqlHelper.Param("@idUsuario", c.IdUsuario),
        SqlHelper.Param("@digito", c.DigitoVerificador));

    public void Modificar(Cliente c) => SqlHelper.EjecutarNonQuery(
        @"UPDATE Clientes SET Nombre=@nombre, Apellido=@apellido, DniEncriptado=@dni,
                 IdUsuario=@idUsuario, DigitoVerificador=@digito
          WHERE IdCliente=@id",
        SqlHelper.Param("@nombre", c.Nombre),
        SqlHelper.Param("@apellido", c.Apellido),
        SqlHelper.Param("@dni", c.DniEncriptado),
        SqlHelper.Param("@idUsuario", c.IdUsuario),
        SqlHelper.Param("@digito", c.DigitoVerificador),
        SqlHelper.Param("@id", c.IdCliente));

    public void Eliminar(int id) => SqlHelper.EjecutarNonQuery(
        "DELETE FROM Clientes WHERE IdCliente = @id", SqlHelper.Param("@id", id));

    public Cliente? ObtenerPorId(int id) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE IdCliente = @id", Mapear, SqlHelper.Param("@id", id));

    public Cliente? ObtenerPorUsuario(int idUsuario) => SqlHelper.EjecutarConsultaUno(
        SelectBase + " WHERE IdUsuario = @idUsuario", Mapear, SqlHelper.Param("@idUsuario", idUsuario));

    public List<Cliente> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        SelectBase + " ORDER BY Apellido, Nombre", Mapear);
}
