using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;

namespace AutoVentas.BLL;

/// <summary>
/// Gestión de Reservas. El Ejecutivo tiene CRUD completo; el Cliente solo puede crear
/// y listar sus propias reservas (ver ObtenerPorCliente).
/// </summary>
public class GestorReservas : GestorNegocioBase<Reserva>
{
    public GestorReservas() : base(new RepositorioReservas(), "Reservas") { }

    protected override int ObtenerId(Reserva entidad) => entidad.IdReserva;

    protected override void Validar(Reserva r)
    {
        if (r.IdVehiculo <= 0) throw new ValidacionException("Debe seleccionar un vehículo.");
        if (r.IdCliente <= 0) throw new ValidacionException("Debe seleccionar un cliente.");
        if (r.FechaVencimiento <= r.FechaReserva) throw new ValidacionException("La fecha de vencimiento debe ser posterior a la fecha de reserva.");
    }

    public List<Reserva> ObtenerPorCliente(int idCliente) => ((RepositorioReservas)Repositorio).ObtenerPorCliente(idCliente);
}
