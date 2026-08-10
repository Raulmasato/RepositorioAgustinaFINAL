namespace AutoVentas.Domain.Entidades;

public class Cliente
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;

    /// <summary>DNI encriptado (AES) por tratarse de un dato sensible (T03).</summary>
    public string DniEncriptado { get; set; } = string.Empty;

    /// <summary>DNI en claro, solo usado en memoria para mostrar/editar en la UI.</summary>
    public string DniPlano { get; set; } = string.Empty;

    public int? IdUsuario { get; set; }
    public string? DigitoVerificador { get; set; }

    public string NombreCompleto => $"{Apellido}, {Nombre}";

    public override string ToString() => NombreCompleto;
}
