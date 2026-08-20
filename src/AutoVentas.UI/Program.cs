using System.Configuration;
using AutoVentas.DAL.Conexion;
using AutoVentas.Services.Bitacora;
using AutoVentas.Services.Excepciones;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Integridad;
using AutoVentas.UI.Formularios;

namespace AutoVentas.UI;

internal static class Program
{
    private static readonly ServicioManejoExcepciones ManejadorExcepciones = new();

    [STAThread]
    private static void Main()
    {
        // T06. Gestión de excepciones: cualquier error no controlado en la UI o en threads
        // secundarios se captura, se serializa en disco y se intenta registrar en bitácora,
        // en lugar de dejar caer el proceso silenciosamente.
        Application.ThreadException += (_, e) => ManejadorExcepciones.Manejar(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception ex) ManejadorExcepciones.Manejar(ex);
        };
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

        // T02. Diferenciación de los procesos de arranque / login / apagado del sistema:
        // el apagado (cierre del proceso, sea por Cerrar sesión, cierre de ventana o Alt+F4)
        // queda registrado igual que el arranque y el login/logout.
        Application.ApplicationExit += (_, _) =>
        {
            try { new ServicioBitacora().Registrar("Apagado del sistema"); }
            catch { /* si la BD ya no está disponible al cerrar, no se debe bloquear el cierre */ }
        };

        ApplicationConfiguration.Initialize();

        InicializarConexionBD();

        // T02. Arranque del sistema: se registra en bitácora, diferenciado del login/logout
        // del usuario (que se registra por separado en GestorAutenticacion) y del apagado
        // (registrado más arriba en Application.ApplicationExit).
        try { new ServicioBitacora().Registrar("Arranque del sistema"); } catch { /* BD recién inicializándose */ }

        // T05. Multi-idioma: se cargan las traducciones desde base de datos antes de mostrar
        // cualquier formulario, para que ya estén disponibles al construir el Login.
        GestorIdioma.Instancia.Inicializar("es");

        // T08. Verificación de integridad de dígitos verificadores ANTES de habilitar el login.
        VerificarIntegridadBaseDeDatos();

        EjecutarFlujoDeAutenticacion();
    }

    private static void InicializarConexionBD()
    {
        var cadena = ConfigurationManager.ConnectionStrings["AutoVentasDB"]?.ConnectionString;
        if (string.IsNullOrWhiteSpace(cadena))
        {
            MessageBox.Show(
                "No se encontró la cadena de conexión 'AutoVentasDB' en App.config.",
                "Error de configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }

        ConexionBD.CadenaConexion = cadena!;
    }

    private static void VerificarIntegridadBaseDeDatos()
    {
        try
        {
            var servicio = new ServicioDigitoVerificador();
            var resultados = servicio.VerificarIntegridad();
            var conAnomalias = resultados.Where(r => !r.Integra).ToList();

            if (conAnomalias.Count > 0)
            {
                var detalle = string.Join(Environment.NewLine,
                    conAnomalias.Select(r => $"- {r.Tabla}: {string.Join(" | ", r.Anomalias)}"));

                MessageBox.Show(
                    "Se detectaron inconsistencias de integridad en la base de datos. " +
                    "Se recomienda contactar al administrador del sistema:" + Environment.NewLine + Environment.NewLine + detalle,
                    "Control de integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            // Si la base de datos todavía no existe (primera ejecución) no se debe impedir el
            // arranque; se informa el detalle para que el usuario revise la conexión configurada.
            MessageBox.Show(
                "No se pudo verificar la integridad de la base de datos. Verifique la conexión configurada en App.config." +
                Environment.NewLine + ex.Message,
                "Control de integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private static void EjecutarFlujoDeAutenticacion()
    {
        while (true)
        {
            using var frmLogin = new FrmLogin();
            var resultado = frmLogin.ShowDialog();

            if (resultado != DialogResult.OK)
            {
                return; // el usuario cerró el login sin autenticarse
            }

            Application.Run(new FrmPrincipal());

            // Al cerrar FrmPrincipal (por Cerrar sesión) se vuelve a mostrar el login.
            if (!FrmPrincipal.SolicitarNuevoLogin)
            {
                return;
            }
            FrmPrincipal.SolicitarNuevoLogin = false;
        }
    }
}
