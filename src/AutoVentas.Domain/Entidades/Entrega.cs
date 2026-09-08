namespace AutoVentas.Domain.Entidades;

public enum EstadoEntrega
{
    Pendiente,
    Entregado,
    Cancelada
}

public class Entrega
{
    public int IdEntrega { get; set; }
    public int IdContrato { get; set; }
    public int IdUsuarioEjecutivo { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string LugarEntrega { get; set; } = string.Empty;
    public EstadoEntrega Estado { get; set; } = EstadoEntrega.Pendiente;
    public string? DigitoVerificador { get; set; }

    public string? ContratoDescripcion { get; set; }
}
