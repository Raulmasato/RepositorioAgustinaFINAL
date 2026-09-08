namespace AutoVentas.Services.Excepciones;

/// <summary>A03. Estructura serializable que representa una excepción no controlada
/// capturada por el sistema, para dejar constancia en disco además de la bitácora.</summary>
[Serializable]
public class RegistroExcepcionSerializado
{
    public DateTime FechaHora { get; set; }
    public string? Usuario { get; set; }
    public string TipoExcepcion { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? ExcepcionInterna { get; set; }
}
