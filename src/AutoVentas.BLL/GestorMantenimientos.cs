using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;

namespace AutoVentas.BLL;

/// <summary>Gestión de Mantenimientos (Técnico).</summary>
public class GestorMantenimientos : GestorNegocioBase<Mantenimiento>
{
    public GestorMantenimientos() : base(new RepositorioMantenimientos(), "Mantenimientos") { }

    protected override int ObtenerId(Mantenimiento entidad) => entidad.IdMantenimiento;

    protected override void Validar(Mantenimiento m)
    {
        if (m.IdVehiculo <= 0) throw new ValidacionException("Debe seleccionar un vehículo.");
        if (m.IdCliente <= 0) throw new ValidacionException("Debe seleccionar un cliente.");
        if (string.IsNullOrWhiteSpace(m.Servicio)) throw new ValidacionException("Debe indicar el servicio realizado.");
        if (m.FechaServicio > DateTime.Now.AddDays(1)) throw new ValidacionException("La fecha del servicio no puede ser futura.");
    }
}
