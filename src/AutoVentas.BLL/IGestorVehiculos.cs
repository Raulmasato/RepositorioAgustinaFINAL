using AutoVentas.Domain.Entidades;

namespace AutoVentas.BLL;

public interface IGestorVehiculos : IGestorNegocioBase<Vehiculo>
{
    List<Vehiculo> ObtenerDisponibles();
}
