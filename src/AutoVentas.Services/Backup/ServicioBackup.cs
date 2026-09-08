using AutoVentas.DAL.Conexion;
using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Seguridad;
using Microsoft.Data.SqlClient;

namespace AutoVentas.Services.Backup;

/// <summary>
/// T07. Gestión de Backup: administra el catálogo de copias de seguridad y dispara las
/// operaciones físicas de BACKUP/RESTORE sobre la base de datos de SQL Server.
/// </summary>
public class ServicioBackup
{
    private readonly RepositorioBackups _repositorio = new();

    public RegistroBackup GenerarBackup(string rutaDestino, string nombreBaseDatos)
    {
        var idUsuario = SesionActual.Instancia.UsuarioLogueado?.IdUsuario;
        try
        {
            EjecutarComandoBackupORestore(
                $"BACKUP DATABASE [{nombreBaseDatos}] TO DISK = @Ruta WITH INIT, STATS = 10",
                rutaDestino);

            var registro = new RegistroBackup
            {
                RutaArchivo = rutaDestino,
                IdUsuario = idUsuario,
                Resultado = "Exitoso",
                Detalle = $"Backup de {nombreBaseDatos} generado correctamente."
            };
            registro.IdBackup = _repositorio.Registrar(registro);
            return registro;
        }
        catch (Exception ex)
        {
            var registro = new RegistroBackup
            {
                RutaArchivo = rutaDestino,
                IdUsuario = idUsuario,
                Resultado = "Error",
                Detalle = ex.Message
            };
            registro.IdBackup = _repositorio.Registrar(registro);
            throw new PersistenciaException("No se pudo generar el backup de la base de datos.", ex);
        }
    }

    public void RestaurarBackup(int idBackup, string nombreBaseDatos)
    {
        var registro = _repositorio.ObtenerPorId(idBackup)
            ?? throw new ValidacionException("El backup seleccionado no existe en el catálogo.");

        try
        {
            CortarConexionesYRestaurar(nombreBaseDatos, registro.RutaArchivo);
        }
        catch (Exception ex)
        {
            throw new PersistenciaException("No se pudo restaurar el backup seleccionado.", ex);
        }
    }

    public List<RegistroBackup> ObtenerCatalogo() => _repositorio.ObtenerCatalogo();

    /// <summary>
    /// Ejecuta BACKUP fuera de una transacción explícita (requisito de SQL Server),
    /// parametrizando la ruta del archivo mediante sp_executesql para evitar inyección SQL.
    /// A diferencia del restore, un backup no necesita acceso exclusivo: SQL Server puede
    /// respaldar una base de datos mientras sigue en uso.
    /// </summary>
    private static void EjecutarComandoBackupORestore(string sqlConParametroRuta, string ruta)
    {
        using var conexion = ConexionBD.CrearConexion();
        using var comando = new SqlCommand(
            "EXEC sp_executesql @Sql, N'@Ruta NVARCHAR(400)', @Ruta = @RutaValor", conexion);
        comando.CommandTimeout = 0; // los backups pueden demorar según el tamaño de la BD
        comando.Parameters.AddWithValue("@Sql", sqlConParametroRuta);
        comando.Parameters.AddWithValue("@RutaValor", ruta);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    /// <summary>
    /// T07. RESTORE DATABASE necesita acceso EXCLUSIVO a la base (a diferencia del backup):
    /// si el propio programa u otra sesión tienen una conexión abierta a
    /// <paramref name="nombreBaseDatos"/>, SQL Server rechaza la restauración con
    /// "No se pudo obtener acceso exclusivo...". Por eso, antes de restaurar, se cortan todas
    /// las conexiones a esa base (las del propio pool de ADO.NET del programa, vía
    /// <see cref="SqlConnection.ClearAllPools"/>, y cualquier otra sesión activa en el
    /// servidor, vía <c>ALTER DATABASE ... SET SINGLE_USER WITH ROLLBACK IMMEDIATE</c>).
    /// Todo esto se ejecuta contra la base <c>master</c>, nunca contra
    /// <paramref name="nombreBaseDatos"/> misma, porque no se puede alterar ni restaurar una
    /// base de datos estando conectado a ella. Al terminar (incluso si el restore falla) se
    /// vuelve a habilitar el acceso multiusuario normal.
    /// </summary>
    private static void CortarConexionesYRestaurar(string nombreBaseDatos, string rutaOrigen)
    {
        SqlConnection.ClearAllPools();

        var cadenaMaster = new SqlConnectionStringBuilder(ConexionBD.CadenaConexion) { InitialCatalog = "master" }.ConnectionString;
        using var conexion = new SqlConnection(cadenaMaster);
        conexion.Open();

        EjecutarNonQuery(conexion, $"ALTER DATABASE [{nombreBaseDatos}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
        try
        {
            using var comandoRestore = new SqlCommand(
                "EXEC sp_executesql @Sql, N'@Ruta NVARCHAR(400)', @Ruta = @RutaValor", conexion);
            comandoRestore.CommandTimeout = 0;
            comandoRestore.Parameters.AddWithValue("@Sql",
                $"RESTORE DATABASE [{nombreBaseDatos}] FROM DISK = @Ruta WITH REPLACE, STATS = 10");
            comandoRestore.Parameters.AddWithValue("@RutaValor", rutaOrigen);
            comandoRestore.ExecuteNonQuery();
        }
        finally
        {
            EjecutarNonQuery(conexion, $"ALTER DATABASE [{nombreBaseDatos}] SET MULTI_USER");
        }
    }

    private static void EjecutarNonQuery(SqlConnection conexion, string sql)
    {
        using var comando = new SqlCommand(sql, conexion) { CommandTimeout = 0 };
        comando.ExecuteNonQuery();
    }
}
