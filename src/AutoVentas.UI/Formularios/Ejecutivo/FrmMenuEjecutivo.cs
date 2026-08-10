using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Menú MDI del rol Ejecutivo: Contratos, Reservas, Pagos, Entregas, Reportes
/// y las herramientas administrativas (Bitácora, Permisos, Backup).</summary>
public class FrmMenuEjecutivo : FormMenuRolBase
{
    protected override string ClaveTituloIdioma => "frm.menuejecutivo";
    protected override string ClaveMenuOpciones => "frm.menuejecutivo";

    public FrmMenuEjecutivo()
    {
        AgregarOpcion("menu.contratos", () => new FrmContratos());
        AgregarOpcion("menu.reservas", () => new FrmReservas());
        AgregarOpcion("menu.pagos", () => new FrmPagos());
        AgregarOpcion("menu.entregas", () => new FrmEntregas());
        AgregarOpcion("menu.reportes", () => new FrmReportes());
        AgregarOpcion("menu.bitacora", () => new FrmBitacora());
        AgregarOpcion("menu.permisos", () => new FrmPermisos());
        AgregarOpcion("menu.backup", () => new FrmBackup());
    }
}
