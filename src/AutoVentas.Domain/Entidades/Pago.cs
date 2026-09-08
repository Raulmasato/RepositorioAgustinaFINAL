namespace AutoVentas.Domain.Entidades;

public class Pago
{
    public int IdPago { get; set; }
    public int IdContrato { get; set; }
    public int IdUsuarioEjecutivo { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public string? DigitoVerificador { get; set; }

    public string? ContratoDescripcion { get; set; }
}
