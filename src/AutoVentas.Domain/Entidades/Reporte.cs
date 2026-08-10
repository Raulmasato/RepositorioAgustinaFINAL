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
}
