namespace AutoVentas.Domain.Entidades;

public class Mantenimiento
{
    public int IdMantenimiento { get; set; }
    public int IdVehiculo { get; set; }
    public int IdCliente { get; set; }
    public string Servicio { get; set; } = string.Empty;
    public DateTime FechaServicio { get; set; }
    public string? DigitoVerificador { get; set; }

    // Datos de solo lectura para mostrar en grillas (se completan via JOIN en el BLL)
    public string? VehiculoDescripcion { get; set; }
    public string? ClienteNombreCompleto { get; set; }
}
