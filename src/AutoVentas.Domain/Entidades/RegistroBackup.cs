namespace AutoVentas.Domain.Entidades;

/// <summary>T07. Entrada del catálogo de copias de seguridad.</summary>
public class RegistroBackup
{
    public int IdBackup { get; set; }
    public string RutaArchivo { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public int? IdUsuario { get; set; }
    public string Resultado { get; set; } = string.Empty; // Exitoso / Error
    public string? Detalle { get; set; }
}
