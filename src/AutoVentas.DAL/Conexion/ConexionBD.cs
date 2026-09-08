using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Conexion;

/// <summary>
/// T01. Punto único de acceso a la cadena de conexión de la base de datos.
/// La capa de presentación la inicializa al arrancar (leyendo App.config) para que
/// el DAL no dependa de System.Configuration ni de ningún detalle de hosting.
/// </summary>
public static class ConexionBD
{
    public static string CadenaConexion { get; set; } = string.Empty;

    public static SqlConnection CrearConexion()
    {
        if (string.IsNullOrWhiteSpace(CadenaConexion))
        {
            throw new InvalidOperationException(
                "La cadena de conexión no fue inicializada. Configure ConexionBD.CadenaConexion al iniciar la aplicación.");
        }

        return new SqlConnection(CadenaConexion);
    }
}
