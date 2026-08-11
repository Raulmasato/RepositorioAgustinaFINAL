using AutoVentas.Services.Ayuda;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Comunes;

/// <summary>D02. Ayuda en línea: lista los temas de las funcionalidades más relevantes del
/// sistema y muestra su explicación al seleccionarlos.</summary>
public class FrmAyuda : Form, IObservadorIdioma
{
    private readonly ListBox _listaTemas = new()
    {
        Left = 0, Top = 0, Width = 220, Dock = DockStyle.Left,
        DisplayMember = nameof(TemaAyuda.Titulo)
    };

    private readonly TextBox _txtContenido = new()
    {
        Dock = DockStyle.Fill,
        Multiline = true,
        ReadOnly = true,
        ScrollBars = ScrollBars.Vertical,
        Font = new Font(FontFamily.GenericSansSerif, 10)
    };

    /// <param name="temaInicial">Clave del tema a mostrar apenas se abre (ayuda contextual).
    /// Si es null, se muestra el primer tema de la lista.</param>
    public FrmAyuda(string? temaInicial = null)
    {
        Width = 700;
        Height = 480;
        StartPosition = FormStartPosition.CenterParent;

        Controls.Add(_txtContenido);
        Controls.Add(_listaTemas);

        _listaTemas.Items.AddRange(ServicioAyuda.ObtenerTemas().Cast<object>().ToArray());
        _listaTemas.SelectedIndexChanged += (_, _) =>
        {
            if (_listaTemas.SelectedItem is TemaAyuda tema)
            {
                _txtContenido.Text = tema.Texto;
            }
        };

        var seleccionado = temaInicial is null
            ? null
            : ServicioAyuda.ObtenerTemas().FirstOrDefault(t => t.Clave == temaInicial);

        _listaTemas.SelectedItem = seleccionado ?? ServicioAyuda.ObtenerTemas().FirstOrDefault();

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        Load += (_, _) => ActualizarIdioma();
    }

    public void ActualizarIdioma()
    {
        Text = GestorIdioma.Instancia.Traducir("menu.ayuda");
    }
}
