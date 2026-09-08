namespace AutoVentas.Services.Idioma;

/// <summary>
/// T05. Patrón Observer: cada formulario/control que necesita refrescar sus textos cuando
/// cambia el idioma implementa esta interfaz y se suscribe a <see cref="GestorIdioma"/>.
/// </summary>
public interface IObservadorIdioma
{
    void ActualizarIdioma();
}
