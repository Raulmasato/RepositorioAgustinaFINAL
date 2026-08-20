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
        AgregarOpcion("menu.contratos", () => new FrmContratos(), "CO004");
        AgregarOpcion("menu.reservas", () => new FrmReservas(), "RE004");
        AgregarOpcion("menu.pagos", () => new FrmPagos(), "PA004");
        AgregarOpcion("menu.entregas", () => new FrmEntregas(), "EN004");
        AgregarOpcion("menu.reportes", () => new FrmReportes(), "RP004");
        AgregarOpcion("menu.bitacora", () => new FrmBitacora(), "AD001");
        AgregarOpcion("menu.permisos", () => new FrmPermisos(), "AD002");
        AgregarOpcion("menu.backup", () => new FrmBackup(), "AD003");
        AgregarOpcion("menu.idiomas", () => new FrmIdiomas(), "AD004");
        AgregarOpcion("menu.historialcambios", () => new FrmHistorialCambios(), "AD005");
    }
}
