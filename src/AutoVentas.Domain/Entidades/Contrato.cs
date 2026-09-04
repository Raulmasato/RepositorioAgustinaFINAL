namespace AutoVentas.Domain.Entidades;

public enum EstadoContrato
{
    Vigente,
    Finalizado,
    Anulado
}

public class Contrato
{
    public int IdContrato { get; set; }
    public int IdVehiculo { get; set; }
    public int IdCliente { get; set; }
    public int IdUsuarioEjecutivo { get; set; }
    public int? IdPresupuesto { get; set; }
    public DateTime FechaContrato { get; set; }
    public decimal Precio { get; set; }
    public EstadoContrato Estado { get; set; } = EstadoContrato.Vigente;
    public string? DigitoVerificador { get; set; }

    public string? VehiculoDescripcion { get; set; }
    public string? ClienteNombreCompleto { get; set; }

    public override string ToString() => $"#{IdContrato} - {VehiculoDescripcion} - {ClienteNombreCompleto} - {Precio:C}";
}
