using AutoVentas.Services.Idioma;
using EntidadIdioma = AutoVentas.Domain.Entidades.Idioma;

namespace AutoVentas.UI.Formularios.Comunes;

/// <summary>ComboBox reutilizable para ELEGIR un idioma (T05). A propósito, seleccionar un
/// idioma acá NO lo aplica todavía — solo lo deja elegido. Aplicar el cambio (que dispara
/// <see cref="GestorIdioma.CambiarIdioma"/> y notifica, patrón Observer, a todos los
/// formularios suscriptos) es responsabilidad del botón "Traducir" de cada pantalla.</summary>
public class SelectorIdioma : ComboBox
{
    public SelectorIdioma()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
        Width = 130;
        DisplayMember = nameof(EntidadIdioma.Nombre);
        ValueMember = nameof(EntidadIdioma.Codigo);

        Items.AddRange(GestorIdioma.Instancia.IdiomasDisponibles.Cast<object>().ToArray());

        var actual = GestorIdioma.Instancia.IdiomasDisponibles.FirstOrDefault(i => i.Codigo == GestorIdioma.Instancia.CodigoIdiomaActual);
        if (actual is not null)
        {
            SelectedItem = actual;
        }
    }
}
