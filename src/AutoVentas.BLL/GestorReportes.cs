using System.Text;
using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;

namespace AutoVentas.BLL;

/// <summary>Gestión de Reportes (Ejecutivo). Al crear un reporte se genera automáticamente
/// su contenido consultando los datos del período y tipo seleccionados.</summary>
public class GestorReportes : GestorNegocioBase<Reporte>
{
    private readonly RepositorioContratos _repositorioContratos = new();
    private readonly RepositorioMantenimientos _repositorioMantenimientos = new();
    private readonly RepositorioPagos _repositorioPagos = new();
    private readonly RepositorioReservas _repositorioReservas = new();

    public GestorReportes() : base(new RepositorioReportes(), "Reportes") { }

    protected override int ObtenerId(Reporte entidad) => entidad.IdReporte;

    protected override void Validar(Reporte r)
    {
        if (string.IsNullOrWhiteSpace(r.Titulo)) throw new ValidacionException("El reporte debe tener un título.");
        if (r.FechaHasta < r.FechaDesde) throw new ValidacionException("El rango de fechas del reporte es inválido.");
    }

    public override int Agregar(Reporte reporte)
    {
        reporte.Contenido = GenerarContenido(reporte.TipoReporte, reporte.FechaDesde, reporte.FechaHasta);
        return base.Agregar(reporte);
    }

    public override void Modificar(Reporte reporte)
    {
        reporte.Contenido = GenerarContenido(reporte.TipoReporte, reporte.FechaDesde, reporte.FechaHasta);
        base.Modificar(reporte);
    }

    private string GenerarContenido(TipoReporte tipo, DateTime desde, DateTime hasta)
    {
        var texto = new StringBuilder();
        texto.AppendLine($"Reporte de {tipo} — período {desde:d} a {hasta:d}");
        texto.AppendLine(new string('-', 60));

        switch (tipo)
        {
            case TipoReporte.Ventas:
                var contratos = _repositorioContratos.ObtenerTodos()
                    .Where(c => c.FechaContrato >= desde && c.FechaContrato <= hasta).ToList();
                texto.AppendLine($"Cantidad de contratos: {contratos.Count}");
                texto.AppendLine($"Monto total vendido: {contratos.Sum(c => c.Precio):C}");
                foreach (var c in contratos)
                {
                    texto.AppendLine($"  #{c.IdContrato} - {c.VehiculoDescripcion} - {c.ClienteNombreCompleto} - {c.Precio:C}");
                }
                break;

            case TipoReporte.Mantenimientos:
                var mantenimientos = _repositorioMantenimientos.ObtenerTodos()
                    .Where(m => m.FechaServicio >= desde && m.FechaServicio <= hasta).ToList();
                texto.AppendLine($"Cantidad de servicios: {mantenimientos.Count}");
                foreach (var m in mantenimientos)
                {
                    texto.AppendLine($"  #{m.IdMantenimiento} - {m.VehiculoDescripcion} - {m.ClienteNombreCompleto} - {m.Servicio}");
                }
                break;

            case TipoReporte.Pagos:
                var pagos = _repositorioPagos.ObtenerTodos()
                    .Where(p => p.FechaPago >= desde && p.FechaPago <= hasta).ToList();
                texto.AppendLine($"Cantidad de pagos: {pagos.Count}");
                texto.AppendLine($"Monto total cobrado: {pagos.Sum(p => p.Monto):C}");
                foreach (var p in pagos)
                {
                    texto.AppendLine($"  #{p.IdPago} - {p.ContratoDescripcion} - {p.Monto:C} ({p.MetodoPago})");
                }
                break;

            case TipoReporte.Reservas:
                var reservas = _repositorioReservas.ObtenerTodos()
                    .Where(r => r.FechaReserva >= desde && r.FechaReserva <= hasta).ToList();
                texto.AppendLine($"Cantidad de reservas: {reservas.Count}");
                foreach (var r in reservas)
                {
                    texto.AppendLine($"  #{r.IdReserva} - {r.VehiculoDescripcion} - {r.ClienteNombreCompleto} - {r.Estado}");
                }
                break;
        }

        return texto.ToString();
    }
}
