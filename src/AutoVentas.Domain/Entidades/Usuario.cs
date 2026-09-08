namespace AutoVentas.Domain.Entidades;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>Hash PBKDF2 de la clave. Nunca se almacena la clave en texto plano.</summary>
    public string ClaveHash { get; set; } = string.Empty;

    /// <summary>Salt aleatorio usado para el hash de la clave.</summary>
    public string ClaveSalt { get; set; } = string.Empty;

    public int IdRol { get; set; }
    public NombreRol Rol { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; }

    /// <summary>Digito verificador horizontal (T08) para control de integridad de la fila.</summary>
    public string? DigitoVerificador { get; set; }
}
