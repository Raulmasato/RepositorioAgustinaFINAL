namespace AutoVentas.Domain.Entidades;

public class Vehiculo
{
    public int IdVehiculo { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int? Anio { get; set; }
    public decimal? Precio { get; set; }
    public bool Disponible { get; set; } = true;
    public string? DigitoVerificador { get; set; }

    public override string ToString() => $"{Marca} {Modelo} ({Color})";
}
