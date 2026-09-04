namespace AutoVentas.Domain.Excepciones;

/// <summary>Se lanza cuando datos ingresados por el usuario no cumplen una regla de negocio.</summary>
[Serializable]
public class ValidacionException : AutoVentasException
{
    public ValidacionException(string mensaje) : base(mensaje) { }
}
