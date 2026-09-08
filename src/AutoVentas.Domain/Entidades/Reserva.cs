namespace AutoVentas.Domain.Entidades;

public enum EstadoReserva
{
    Pendiente,
    Confirmada,
    Cancelada
}

public class Reserva
{
    public int IdReserva { get; set; }
    public int IdVehiculo { get; set; }
    public int IdCliente { get; set; }
    public int? IdUsuarioEjecutivo { get; set; }
    public DateTime FechaReserva { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;
    public string? DigitoVerificador { get; set; }

    public string? VehiculoDescripcion { get; set; }
    public string? ClienteNombreCompleto { get; set; }
}
