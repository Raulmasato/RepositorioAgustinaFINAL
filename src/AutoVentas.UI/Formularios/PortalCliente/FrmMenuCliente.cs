using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.PortalCliente;

/// <summary>Menú MDI del rol Cliente: catálogo de vehículos y sus propias reservas.</summary>
public class FrmMenuCliente : FormMenuRolBase
{
    protected override string ClaveTituloIdioma => "frm.menucliente";
    protected override string ClaveMenuOpciones => "frm.menucliente";

    public FrmMenuCliente()
    {
        AgregarOpcion("menu.vehiculos", () => new FrmCatalogoVehiculos());
        AgregarOpcion("menu.reservas", () => new FrmMisReservas());
    }
}
