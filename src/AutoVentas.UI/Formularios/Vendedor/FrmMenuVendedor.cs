using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Vendedor;

/// <summary>Menú MDI del rol Vendedor: Presupuestos, Vehículos y Clientes.</summary>
public class FrmMenuVendedor : FormMenuRolBase
{
    protected override string ClaveTituloIdioma => "frm.menuvendedor";
    protected override string ClaveMenuOpciones => "frm.menuvendedor";

    public FrmMenuVendedor()
    {
        AgregarOpcion("menu.presupuestos", () => new FrmPresupuestos(), "PR004");
        AgregarOpcion("menu.vehiculos", () => new FrmVehiculos(), "VE004");
        AgregarOpcion("menu.clientes", () => new FrmClientes(), "CL004");
    }
}
