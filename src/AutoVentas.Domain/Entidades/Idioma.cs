namespace AutoVentas.Domain.Entidades;

/// <summary>T05. Idioma soportado por el sistema. Las traducciones viven en base de datos,
/// no en archivos de recursos estáticos, para poder incorporar idiomas en caliente.</summary>
public class Idioma
{
    public int IdIdioma { get; set; }
    public string Codigo { get; set; } = string.Empty; // es, en, pt, fr...
    public string Nombre { get; set; } = string.Empty;
}

public class Traduccion
{
    public int IdTraduccion { get; set; }
    public int IdIdioma { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
}
