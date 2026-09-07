using AutoVentas.Domain.Entidades;

namespace AutoVentas.BLL;

public interface IGestorReservas : IGestorNegocioBase<Reserva>
{
    List<Reserva> ObtenerPorCliente(int idCliente);
}
