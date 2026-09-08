namespace AutoVentas.Domain.Excepciones;

/// <summary>Errores de autenticación, autorización o violaciones de integridad (T08).</summary>
[Serializable]
public class SeguridadException : AutoVentasException
{
    public SeguridadException(string mensaje) : base(mensaje) { }
}
