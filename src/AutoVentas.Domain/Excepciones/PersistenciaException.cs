namespace AutoVentas.Domain.Excepciones;

/// <summary>Envuelve errores de la capa de acceso a datos (conexión, SQL, etc.)
/// para que las capas superiores no dependan de tipos específicos de ADO.NET.</summary>
[Serializable]
public class PersistenciaException : AutoVentasException
{
    public PersistenciaException(string mensaje, Exception inner) : base(mensaje, inner) { }
}
