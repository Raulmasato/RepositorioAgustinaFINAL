using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Tecnico;

/// <summary>Menú MDI del rol Técnico: Mantenimientos.</summary>
public class FrmMenuTecnico : FormMenuRolBase
{
    protected override string ClaveTituloIdioma => "frm.menutecnico";
    protected override string ClaveMenuOpciones => "frm.menutecnico";

    public FrmMenuTecnico()
    {
        AgregarOpcion("menu.mantenimientos", () => new FrmMantenimientos());
    }
}
