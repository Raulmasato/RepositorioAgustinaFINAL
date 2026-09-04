using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;

namespace AutoVentas.BLL;

/// <summary>Gestión de Contratos (Ejecutivo). Un contrato puede originarse en un presupuesto (&lt;&lt;include&gt;&gt;).</summary>
public class GestorContratos : GestorNegocioBase<Contrato>
{
    public GestorContratos() : base(new RepositorioContratos(), "Contratos") { }

    protected override int ObtenerId(Contrato entidad) => entidad.IdContrato;

    protected override void Validar(Contrato c)
    {
        if (c.IdVehiculo <= 0) throw new ValidacionException("Debe seleccionar un vehículo.");
        if (c.IdCliente <= 0) throw new ValidacionException("Debe seleccionar un cliente.");
        if (c.IdUsuarioEjecutivo <= 0) throw new ValidacionException("El contrato debe estar asociado a un ejecutivo.");
        if (c.Precio <= 0) throw new ValidacionException("El precio del contrato debe ser mayor a cero.");
    }
}
