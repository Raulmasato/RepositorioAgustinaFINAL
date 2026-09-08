namespace AutoVentas.Domain.Entidades;

public enum NombreRol
{
    Cliente = 1,
    Vendedor = 2,
    Tecnico = 3,
    Ejecutivo = 4
}

public class Rol
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
