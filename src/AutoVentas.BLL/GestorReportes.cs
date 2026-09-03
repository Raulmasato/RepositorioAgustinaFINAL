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
                var todosContratos = _repositorioContratos.ObtenerTodos();
                var contratos = todosContratos
                    .Where(c => c.FechaContrato >= desde && c.FechaContrato <= hasta).ToList();

                AgregarEstadisticas(texto, contratos.Count, todosContratos.Count, "contratos", sb =>
                {
                    if (contratos.Count == 0) return;
                    var montoPeriodo = contratos.Sum(x => x.Precio);
                    var montoHistorico = todosContratos.Sum(x => x.Precio);
                    sb.AppendLine($"Monto total vendido: {montoPeriodo:C} ({Porcentaje(montoPeriodo, montoHistorico)} del monto histórico)");
                    sb.AppendLine($"Precio promedio: {contratos.Average(x => x.Precio):C}");
                    sb.AppendLine($"Precio máximo: {contratos.Max(x => x.Precio):C}");
                    sb.AppendLine($"Precio mínimo: {contratos.Min(x => x.Precio):C}");
                    sb.AppendLine("Por estado:");
                    foreach (var g in contratos.GroupBy(x => x.Estado).OrderByDescending(g => g.Count()))
                    {
                        sb.AppendLine($"  {g.Key}: {g.Count()} contratos ({Porcentaje(g.Count(), contratos.Count)}) - {g.Sum(x => x.Precio):C}");
                    }
                    var vehiculoTop = contratos.GroupBy(x => x.VehiculoDescripcion).OrderByDescending(g => g.Count()).First();
                    sb.AppendLine($"Vehículo más vendido: {vehiculoTop.Key} ({vehiculoTop.Count()} ventas, {Porcentaje(vehiculoTop.Count(), contratos.Count)})");
                    var clienteTop = contratos.GroupBy(x => x.ClienteNombreCompleto).OrderByDescending(g => g.Count()).First();
                    sb.AppendLine($"Cliente con más compras: {clienteTop.Key} ({clienteTop.Count()} compras, {Porcentaje(clienteTop.Count(), contratos.Count)})");
                });

                texto.AppendLine();
                texto.AppendLine("Detalle:");
                foreach (var c in contratos)
                {
                    texto.AppendLine($"  #{c.IdContrato} - {c.VehiculoDescripcion} - {c.ClienteNombreCompleto} - {c.Precio:C} ({c.Estado})");
                }
                break;

            case TipoReporte.Mantenimientos:
                var todosMantenimientos = _repositorioMantenimientos.ObtenerTodos();
                var mantenimientos = todosMantenimientos
                    .Where(m => m.FechaServicio >= desde && m.FechaServicio <= hasta).ToList();

                AgregarEstadisticas(texto, mantenimientos.Count, todosMantenimientos.Count, "servicios", sb =>
                {
                    if (mantenimientos.Count == 0) return;
                    sb.AppendLine("Por tipo de servicio:");
                    foreach (var g in mantenimientos.GroupBy(x => x.Servicio).OrderByDescending(g => g.Count()))
                    {
                        sb.AppendLine($"  {g.Key}: {g.Count()} ({Porcentaje(g.Count(), mantenimientos.Count)})");
                    }
                    var vehiculoTop = mantenimientos.GroupBy(x => x.VehiculoDescripcion).OrderByDescending(g => g.Count()).First();
                    sb.AppendLine($"Vehículo con más mantenimientos: {vehiculoTop.Key} ({vehiculoTop.Count()} servicios, {Porcentaje(vehiculoTop.Count(), mantenimientos.Count)})");
                });

                texto.AppendLine();
                texto.AppendLine("Detalle:");
                foreach (var m in mantenimientos)
                {
                    texto.AppendLine($"  #{m.IdMantenimiento} - {m.VehiculoDescripcion} - {m.ClienteNombreCompleto} - {m.Servicio}");
                }
                break;

            case TipoReporte.Pagos:
                var todosPagos = _repositorioPagos.ObtenerTodos();
                var pagos = todosPagos
                    .Where(p => p.FechaPago >= desde && p.FechaPago <= hasta).ToList();

                AgregarEstadisticas(texto, pagos.Count, todosPagos.Count, "pagos", sb =>
                {
                    if (pagos.Count == 0) return;
                    var montoPeriodo = pagos.Sum(x => x.Monto);
                    var montoHistorico = todosPagos.Sum(x => x.Monto);
                    sb.AppendLine($"Monto total cobrado: {montoPeriodo:C} ({Porcentaje(montoPeriodo, montoHistorico)} del monto histórico)");
                    sb.AppendLine($"Monto promedio: {pagos.Average(x => x.Monto):C}");
                    sb.AppendLine("Por método de pago:");
                    foreach (var g in pagos.GroupBy(x => x.MetodoPago).OrderByDescending(g => g.Count()))
                    {
                        sb.AppendLine($"  {g.Key}: {g.Count()} pagos ({Porcentaje(g.Count(), pagos.Count)}) - {g.Sum(x => x.Monto):C}");
                    }
                });

                texto.AppendLine();
                texto.AppendLine("Detalle:");
                foreach (var p in pagos)
                {
                    texto.AppendLine($"  #{p.IdPago} - {p.ContratoDescripcion} - {p.Monto:C} ({p.MetodoPago})");
                }
                break;

            case TipoReporte.Reservas:
                var todasReservas = _repositorioReservas.ObtenerTodos();
                var reservas = todasReservas
                    .Where(r => r.FechaReserva >= desde && r.FechaReserva <= hasta).ToList();

                AgregarEstadisticas(texto, reservas.Count, todasReservas.Count, "reservas", sb =>
                {
                    if (reservas.Count == 0) return;
                    sb.AppendLine("Por estado:");
                    foreach (var g in reservas.GroupBy(x => x.Estado).OrderByDescending(g => g.Count()))
                    {
                        sb.AppendLine($"  {g.Key}: {g.Count()} ({Porcentaje(g.Count(), reservas.Count)})");
                    }
                    var vehiculoTop = reservas.GroupBy(x => x.VehiculoDescripcion).OrderByDescending(g => g.Count()).First();
                    sb.AppendLine($"Vehículo más reservado: {vehiculoTop.Key} ({vehiculoTop.Count()} reservas, {Porcentaje(vehiculoTop.Count(), reservas.Count)})");
                });

                texto.AppendLine();
                texto.AppendLine("Detalle:");
                foreach (var r in reservas)
                {
                    texto.AppendLine($"  #{r.IdReserva} - {r.VehiculoDescripcion} - {r.ClienteNombreCompleto} - {r.Estado}");
                }
                break;
        }

        return texto.ToString();
    }

    /// <summary>Agrega la sección "Estadísticas" común a los cuatro tipos de reporte: la
    /// cantidad de registros del período y qué porcentaje representa sobre el total histórico
    /// (todos los registros de la tabla, sin filtrar por fecha), seguida de las estadísticas
    /// específicas del tipo (agregarEspecificas), o un aviso si no hay datos en el rango de
    /// fechas elegido.</summary>
    private static void AgregarEstadisticas(StringBuilder texto, int cantidad, int cantidadHistorica, string sustantivoPlural, Action<StringBuilder> agregarEspecificas)
    {
        texto.AppendLine("Estadísticas:");
        texto.AppendLine($"Cantidad de {sustantivoPlural} en el período: {cantidad} ({Porcentaje(cantidad, cantidadHistorica)} del total histórico de {cantidadHistorica})");

        if (cantidad == 0)
        {
            texto.AppendLine("No hay datos en el período seleccionado.");
            return;
        }

        agregarEspecificas(texto);
    }

    /// <summary>Calcula qué porcentaje representa <paramref name="parte"/> sobre
    /// <paramref name="total"/>, devuelto ya formateado (por ejemplo "35,0 %"). Si el total es
    /// cero (no hay ningún registro histórico) devuelve "N/D" para evitar una división por cero.</summary>
    private static string Porcentaje(decimal parte, decimal total) =>
        total == 0 ? "N/D" : $"{parte / total * 100:0.0} %";

    private static string Porcentaje(int parte, int total) => Porcentaje((decimal)parte, total);
}
