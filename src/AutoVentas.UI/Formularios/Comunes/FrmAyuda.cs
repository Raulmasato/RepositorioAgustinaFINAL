using AutoVentas.Services.Ayuda;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Comunes;

/// <summary>D02. Ayuda en línea: lista los temas de las funcionalidades más relevantes del
/// sistema y muestra su explicación al seleccionarlos.</summary>
public partial class FrmAyuda : Form, IObservadorIdioma
{
    /// <param name="temaInicial">Clave del tema a mostrar apenas se abre (ayuda contextual).
    /// Si es null, se muestra el primer tema de la lista.</param>
    public FrmAyuda(string? temaInicial = null)
    {
        InitializeComponent();

        _listaTemas.Items.AddRange(ServicioAyuda.ObtenerTemas().Cast<object>().ToArray());

        var seleccionado = temaInicial is null
            ? null
            : ServicioAyuda.ObtenerTemas().FirstOrDefault(t => t.Clave == temaInicial);

        _listaTemas.SelectedItem = seleccionado ?? ServicioAyuda.ObtenerTemas().FirstOrDefault();

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        Load += (_, _) => ActualizarIdioma();
    }

    private void ListaTemas_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_listaTemas.SelectedItem is TemaAyuda tema)
        {
            _txtContenido.Text = tema.Texto;
        }
    }

    public void ActualizarIdioma()
    {
        Text = GestorIdioma.Instancia.Traducir("menu.ayuda");
    }
}
