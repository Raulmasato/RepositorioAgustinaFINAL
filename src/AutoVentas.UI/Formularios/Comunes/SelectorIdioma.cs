using AutoVentas.Services.Idioma;
using EntidadIdioma = AutoVentas.Domain.Entidades.Idioma;

namespace AutoVentas.UI.Formularios.Comunes;

/// <summary>ComboBox reutilizable para cambiar el idioma activo (T05). Al seleccionar un
/// idioma dispara <see cref="GestorIdioma.CambiarIdioma"/>, que notifica (Observer) a todos
/// los formularios suscriptos para que refresquen sus textos.</summary>
public class SelectorIdioma : ComboBox
{
    public SelectorIdioma()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
        Width = 130;
        DisplayMember = nameof(EntidadIdioma.Nombre);
        ValueMember = nameof(EntidadIdioma.Codigo);

        Items.AddRange(GestorIdioma.Instancia.IdiomasDisponibles.Cast<object>().ToArray());
        SelectedIndexChanged += (_, _) =>
        {
            if (SelectedItem is EntidadIdioma idioma)
            {
                GestorIdioma.Instancia.CambiarIdioma(idioma.Codigo);
            }
        };

        var actual = GestorIdioma.Instancia.IdiomasDisponibles.FirstOrDefault(i => i.Codigo == GestorIdioma.Instancia.CodigoIdiomaActual);
        if (actual is not null)
        {
            SelectedItem = actual;
        }
    }
}
