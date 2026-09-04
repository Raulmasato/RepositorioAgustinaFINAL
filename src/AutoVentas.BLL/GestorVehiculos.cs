using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;

namespace AutoVentas.BLL;

/// <summary>Gestión de Vehículos (Vendedor: alta de inventario, consultado por Ejecutivo/Cliente).</summary>
public class GestorVehiculos : GestorNegocioBase<Vehiculo>
{
    public GestorVehiculos() : base(new RepositorioVehiculos(), "Vehiculos") { }

    protected override int ObtenerId(Vehiculo entidad) => entidad.IdVehiculo;

    protected override void Validar(Vehiculo v)
    {
        if (string.IsNullOrWhiteSpace(v.Marca)) throw new ValidacionException("La marca del vehículo es obligatoria.");
        if (string.IsNullOrWhiteSpace(v.Modelo)) throw new ValidacionException("El modelo del vehículo es obligatorio.");
        if (string.IsNullOrWhiteSpace(v.Color)) throw new ValidacionException("El color del vehículo es obligatorio.");
        if (v.Precio is < 0) throw new ValidacionException("El precio del vehículo no puede ser negativo.");
        if (v.Anio is < 1950 or > 2100) throw new ValidacionException("El año del vehículo no es válido.");
    }

    public List<Vehiculo> ObtenerDisponibles() => ((RepositorioVehiculos)Repositorio).ObtenerDisponibles();
}
