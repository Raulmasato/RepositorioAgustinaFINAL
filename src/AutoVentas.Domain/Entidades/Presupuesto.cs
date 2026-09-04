namespace AutoVentas.Domain.Entidades;

public enum EstadoPresupuesto
{
    Pendiente,
    Aprobado,
    Rechazado
}

public class Presupuesto
{
    public int IdPresupuesto { get; set; }
    public int IdVehiculo { get; set; }
    public int IdCliente { get; set; }
    public int IdUsuarioVendedor { get; set; }
    public DateTime FechaPresupuesto { get; set; }
    public decimal Monto { get; set; }
    public EstadoPresupuesto Estado { get; set; } = EstadoPresupuesto.Pendiente;
    public string? DigitoVerificador { get; set; }

    public string? VehiculoDescripcion { get; set; }
    public string? ClienteNombreCompleto { get; set; }

    public override string ToString() => $"#{IdPresupuesto} - {VehiculoDescripcion} - {ClienteNombreCompleto} - {Monto:C}";
}
