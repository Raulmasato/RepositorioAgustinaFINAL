namespace AutoVentas.Domain.Entidades;

/// <summary>T06b. Auditoría campo a campo: quién, cuándo y qué cambió en una entidad.</summary>
public class RegistroControlCambio
{
    public long IdControlCambio { get; set; }
    public string Tabla { get; set; } = string.Empty;
    public int IdRegistro { get; set; }
    public string Campo { get; set; } = string.Empty;
    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }
    public string TipoOperacion { get; set; } = string.Empty; // INSERT / UPDATE / DELETE
    public int? IdUsuario { get; set; }
    public DateTime FechaHora { get; set; }

    public string? NombreUsuario { get; set; }
}
