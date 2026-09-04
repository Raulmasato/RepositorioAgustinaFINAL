using AutoVentas.Domain.Excepciones;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Conexion;

/// <summary>
/// Helper de acceso a datos: centraliza la apertura de conexión, el manejo de comandos
/// parametrizados (previene inyección SQL) y la traducción de errores de ADO.NET a
/// excepciones de dominio, para que BLL/Servicios no dependan de Microsoft.Data.SqlClient.
/// </summary>
public static class SqlHelper
{
    public static int EjecutarNonQuery(string sql, params SqlParameter[] parametros)
    {
        try
        {
            using var conexion = ConexionBD.CrearConexion();
            using var comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddRange(parametros);
            conexion.Open();
            return comando.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            throw new PersistenciaException($"Error de base de datos ejecutando comando: {ex.Message}", ex);
        }
    }

    /// <summary>Ejecuta un INSERT y devuelve el identificador generado (SCOPE_IDENTITY).</summary>
    public static int EjecutarInsertYObtenerId(string sql, params SqlParameter[] parametros)
    {
        try
        {
            using var conexion = ConexionBD.CrearConexion();
            using var comando = new SqlCommand(sql + "; SELECT CAST(SCOPE_IDENTITY() AS INT);", conexion);
            comando.Parameters.AddRange(parametros);
            conexion.Open();
            var resultado = comando.ExecuteScalar();
            return resultado is null or DBNull ? 0 : Convert.ToInt32(resultado);
        }
        catch (SqlException ex)
        {
            throw new PersistenciaException($"Error de base de datos insertando registro: {ex.Message}", ex);
        }
    }

    public static object? EjecutarScalar(string sql, params SqlParameter[] parametros)
    {
        try
        {
            using var conexion = ConexionBD.CrearConexion();
            using var comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddRange(parametros);
            conexion.Open();
            return comando.ExecuteScalar();
        }
        catch (SqlException ex)
        {
            throw new PersistenciaException($"Error de base de datos ejecutando consulta escalar: {ex.Message}", ex);
        }
    }

    public static List<T> EjecutarConsulta<T>(string sql, Func<SqlDataReader, T> mapear, params SqlParameter[] parametros)
    {
        try
        {
            var resultado = new List<T>();
            using var conexion = ConexionBD.CrearConexion();
            using var comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddRange(parametros);
            conexion.Open();
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                resultado.Add(mapear(lector));
            }
            return resultado;
        }
        catch (SqlException ex)
        {
            throw new PersistenciaException($"Error de base de datos ejecutando consulta: {ex.Message}", ex);
        }
    }

    public static T? EjecutarConsultaUno<T>(string sql, Func<SqlDataReader, T> mapear, params SqlParameter[] parametros)
        where T : class
    {
        return EjecutarConsulta(sql, mapear, parametros).FirstOrDefault();
    }

    public static SqlParameter Param(string nombre, object? valor)
        => new(nombre, valor ?? DBNull.Value);
}
