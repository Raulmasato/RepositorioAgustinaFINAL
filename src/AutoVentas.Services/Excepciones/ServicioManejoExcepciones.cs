using System.Xml.Serialization;
using AutoVentas.Services.Bitacora;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.Services.Excepciones;

/// <summary>
/// Gestión de excepciones centralizada: toda excepción no controlada que llega hasta la UI
/// (ver Program.cs) pasa por acá. Se intenta dejar rastro en dos lugares independientes,
/// para que un problema con la base de datos no implique perder el registro del error:
///   1) Bitácora (BD) — si la conexión está disponible.
///   2) Archivo serializado en disco (A03) — siempre, como respaldo.
/// </summary>
public class ServicioManejoExcepciones
{
    private readonly string _carpetaLogs;

    public ServicioManejoExcepciones(string? carpetaLogs = null)
    {
        _carpetaLogs = carpetaLogs ?? Path.Combine(AppContext.BaseDirectory, "Logs");
    }

    public void Manejar(Exception excepcion)
    {
        var registro = new RegistroExcepcionSerializado
        {
            FechaHora = DateTime.Now,
            Usuario = SesionActual.Instancia.UsuarioLogueado?.NombreUsuario,
            TipoExcepcion = excepcion.GetType().FullName ?? excepcion.GetType().Name,
            Mensaje = excepcion.Message,
            StackTrace = excepcion.StackTrace,
            ExcepcionInterna = excepcion.InnerException?.Message
        };

        SerializarAArchivo(registro);
        IntentarRegistrarEnBitacora(registro);
    }

    private void SerializarAArchivo(RegistroExcepcionSerializado registro)
    {
        try
        {
            Directory.CreateDirectory(_carpetaLogs);
            var nombreArchivo = $"excepcion_{registro.FechaHora:yyyyMMdd_HHmmssfff}.xml";
            var rutaCompleta = Path.Combine(_carpetaLogs, nombreArchivo);

            var serializador = new XmlSerializer(typeof(RegistroExcepcionSerializado));
            using var flujo = File.Create(rutaCompleta);
            serializador.Serialize(flujo, registro);
        }
        catch
        {
            // Si ni siquiera se puede escribir en disco no hay mucho más por hacer:
            // se evita relanzar para no ocultar la excepción original que disparó este manejo.
        }
    }

    private static void IntentarRegistrarEnBitacora(RegistroExcepcionSerializado registro)
    {
        try
        {
            new ServicioBitacora().Registrar("Excepción no controlada",
                $"{registro.TipoExcepcion}: {registro.Mensaje}");
        }
        catch
        {
            // La bitácora vive en la misma base de datos que puede haber causado el error;
            // no se debe generar una excepción en cadena dentro del manejador de excepciones.
        }
    }
}
