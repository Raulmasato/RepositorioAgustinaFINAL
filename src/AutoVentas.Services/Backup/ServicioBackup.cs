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
            EjecutarComandoBackupORestore(
                $"RESTORE DATABASE [{nombreBaseDatos}] FROM DISK = @Ruta WITH REPLACE, STATS = 10",
                registro.RutaArchivo);
        }
        catch (Exception ex)
        {
            throw new PersistenciaException("No se pudo restaurar el backup seleccionado.", ex);
        }
    }

    public List<RegistroBackup> ObtenerCatalogo() => _repositorio.ObtenerCatalogo();

    /// <summary>
    /// Ejecuta BACKUP/RESTORE fuera de una transacción explícita (requisito de SQL Server),
    /// parametrizando la ruta del archivo mediante sp_executesql para evitar inyección SQL.
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
}
