namespace AutoVentas.Domain.Entidades;

public enum TipoReporte
{
    Ventas,
    Mantenimientos,
    Pagos,
    Reservas
}

public class Reporte
{
    public int IdReporte { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public TipoReporte TipoReporte { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime FechaHasta { get; set; }
    public string? Contenido { get; set; }
    public int IdUsuarioEjecutivo { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public string? DigitoVerificador { get; set; }

    /// <summary>Porcentaje que la cantidad de registros del período representa sobre el total
    /// histórico de la tabla correspondiente (por ejemplo, 30 = "30% de todos los contratos
    /// registrados corresponden a este período"). Null si el reporte es de antes de esta
    /// funcionalidad y todavía no fue regenerado.</summary>
    public decimal? PorcentajeCantidad { get; set; }

    /// <summary>Igual que <see cref="PorcentajeCantidad"/> pero sobre el monto (solo aplica a
    /// Ventas y Pagos, que manejan un importe; queda null en Mantenimientos y Reservas).</summary>
    public decimal? PorcentajeMonto { get; set; }
}
