namespace AutoVentas.Domain.Excepciones;

/// <summary>Excepción base de todo el sistema. Permite distinguir errores esperados
/// del negocio de excepciones no controladas (T06 gestión de excepciones).</summary>
[Serializable]
public class AutoVentasException : Exception
{
    public AutoVentasException() { }
    public AutoVentasException(string mensaje) : base(mensaje) { }
    public AutoVentasException(string mensaje, Exception inner) : base(mensaje, inner) { }
}
