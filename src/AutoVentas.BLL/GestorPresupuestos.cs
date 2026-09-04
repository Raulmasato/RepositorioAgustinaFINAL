using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;

namespace AutoVentas.BLL;

/// <summary>Gestión de Presupuestos (Vendedor).</summary>
public class GestorPresupuestos : GestorNegocioBase<Presupuesto>
{
    public GestorPresupuestos() : base(new RepositorioPresupuestos(), "Presupuestos") { }

    protected override int ObtenerId(Presupuesto entidad) => entidad.IdPresupuesto;

    protected override void Validar(Presupuesto p)
    {
        if (p.IdVehiculo <= 0) throw new ValidacionException("Debe seleccionar un vehículo.");
        if (p.IdCliente <= 0) throw new ValidacionException("Debe seleccionar un cliente.");
        if (p.Monto <= 0) throw new ValidacionException("El monto del presupuesto debe ser mayor a cero.");
    }
}
