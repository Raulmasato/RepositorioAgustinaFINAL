using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;

namespace AutoVentas.BLL;

/// <summary>Gestión de Pagos (Ejecutivo).</summary>
public class GestorPagos : GestorNegocioBase<Pago>
{
    public GestorPagos() : base(new RepositorioPagos(), "Pagos") { }

    protected override int ObtenerId(Pago entidad) => entidad.IdPago;

    protected override void Validar(Pago p)
    {
        if (p.IdContrato <= 0) throw new ValidacionException("Debe seleccionar un contrato.");
        if (p.Monto <= 0) throw new ValidacionException("El monto del pago debe ser mayor a cero.");
        if (string.IsNullOrWhiteSpace(p.MetodoPago)) throw new ValidacionException("Debe indicar el método de pago.");
    }
}
