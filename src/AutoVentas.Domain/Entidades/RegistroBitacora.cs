namespace AutoVentas.Domain.Entidades;

/// <summary>T06a. Registro de una operación realizada por un usuario dentro del sistema.</summary>
public class RegistroBitacora
{
    public long IdBitacora { get; set; }
    public DateTime FechaHora { get; set; }
    public int? IdUsuario { get; set; }
    public string Actividad { get; set; } = string.Empty;
    public string? Informacion { get; set; }

    public string? NombreUsuario { get; set; }
}
