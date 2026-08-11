namespace AutoVentas.Services.Ayuda;

/// <summary>Un tema de ayuda: título corto para la lista y texto explicativo completo.</summary>
public record TemaAyuda(string Clave, string Titulo, string Texto);

/// <summary>
/// D02. Ayuda en línea.
/// Mantiene, en un único lugar, el contenido de ayuda de las funcionalidades más relevantes
/// del sistema, para que <c>FrmAyuda</c> lo muestre sin acoplar la capa de presentación a los
/// textos en sí (mismo criterio de reuso que el resto de los servicios transversales).
/// </summary>
public static class ServicioAyuda
{
    public static IReadOnlyList<TemaAyuda> ObtenerTemas() => Temas;

    private static readonly List<TemaAyuda> Temas = new()
    {
        new TemaAyuda("login",
            "Inicio de sesión",
            "Ingresá tu nombre de usuario y contraseña y presioná \"Ingresar\". Si todavía no " +
            "tenés una cuenta, usá el botón \"Registrarse\" para crear un usuario nuevo (podés " +
            "registrarte como Cliente, Vendedor, Técnico o Ejecutivo). El botón \"Traducir\" " +
            "cambia el idioma de toda la aplicación al que hayas elegido en el combo de al lado."),

        new TemaAyuda("idioma",
            "Cambiar de idioma",
            "En cualquier pantalla que tenga el combo de idioma, elegí el idioma deseado. El " +
            "cambio se aplica de inmediato a todos los formularios que tengas abiertos en ese " +
            "momento, sin necesidad de reiniciar el programa."),

        new TemaAyuda("vehiculos",
            "Gestión de Vehículos",
            "Permite dar de alta, modificar, eliminar y listar los vehículos del inventario " +
            "(marca, modelo, color, año, precio y disponibilidad). Los vehículos marcados como " +
            "\"Disponible\" son los que los Clientes pueden ver en el catálogo y reservar."),

        new TemaAyuda("clientes",
            "Gestión de Clientes",
            "Permite dar de alta, modificar, eliminar y listar los clientes (nombre, apellido y " +
            "DNI). El DNI se guarda encriptado en la base de datos por ser un dato sensible."),

        new TemaAyuda("mantenimientos",
            "Gestión de Mantenimientos",
            "Registra los servicios de mantenimiento realizados a un vehículo para un cliente " +
            "determinado (por ejemplo: cambio de aceite, inflado de neumáticos, reparación de " +
            "un golpe), junto con la fecha en la que se realizó."),

        new TemaAyuda("presupuestos",
            "Gestión de Presupuestos",
            "El Vendedor genera un presupuesto para un cliente sobre un vehículo puntual, con un " +
            "monto y un estado (Pendiente/Aprobado/Rechazado). Un presupuesto aprobado puede " +
            "usarse luego como base de un Contrato."),

        new TemaAyuda("contratos",
            "Gestión de Contratos",
            "El Ejecutivo formaliza la venta de un vehículo a un cliente. Un contrato puede " +
            "originarse en un presupuesto ya aprobado (opcional) e incluye el precio final y el " +
            "estado del contrato (Vigente/Finalizado/Anulado)."),

        new TemaAyuda("reservas",
            "Gestión de Reservas",
            "El Ejecutivo tiene control total sobre las reservas (crear, modificar, eliminar y " +
            "listar). El Cliente, desde su propio menú, solo puede crear reservas sobre " +
            "vehículos disponibles y ver el listado de sus propias reservas."),

        new TemaAyuda("pagos",
            "Gestión de Pagos",
            "Registra los pagos asociados a un contrato: monto, fecha y método de pago " +
            "(efectivo, transferencia, tarjeta, etc.)."),

        new TemaAyuda("entregas",
            "Gestión de Entregas",
            "Coordina la entrega física del vehículo vinculado a un contrato: fecha, lugar y " +
            "estado de la entrega (Pendiente/Entregado/Cancelada)."),

        new TemaAyuda("reportes",
            "Gestión de Reportes",
            "Genera reportes de Ventas, Mantenimientos, Pagos o Reservas para un rango de " +
            "fechas; el contenido se arma automáticamente a partir de los datos del sistema. " +
            "Desde el botón \"Exportar a PDF\" podés guardar cualquier reporte como un archivo " +
            "PDF real en tu computadora."),

        new TemaAyuda("bitacora",
            "Bitácora",
            "Muestra el historial de operaciones realizadas por los usuarios del sistema " +
            "(fecha, usuario, actividad). Se puede buscar combinando texto de actividad y un " +
            "rango de fechas."),

        new TemaAyuda("permisos",
            "Permisos por rol",
            "Permite definir, para cada rol (Cliente, Vendedor, Técnico, Ejecutivo), qué " +
            "permisos tiene asignados, usando un árbol donde tildar un permiso compuesto tilda " +
            "automáticamente todos los permisos que agrupa."),

        new TemaAyuda("backup",
            "Copias de seguridad",
            "Permite generar una copia de seguridad (\"backup\") de la base de datos completa " +
            "en un archivo, y restaurar una copia anterior en caso de ser necesario. Todas las " +
            "copias generadas quedan registradas en un catálogo."),
    };
}
