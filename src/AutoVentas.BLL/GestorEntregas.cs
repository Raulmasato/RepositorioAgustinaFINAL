using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;

namespace AutoVentas.BLL;

/// <summary>Gestión de Entregas (Ejecutivo). Una entrega &lt;&lt;include&gt;&gt; la gestión de pagos.</summary>
public class GestorEntregas : GestorNegocioBase<Entrega>
{
    public GestorEntregas() : base(new RepositorioEntregas(), "Entregas") { }

    protected override int ObtenerId(Entrega entidad) => entidad.IdEntrega;

    protected override void Validar(Entrega e)
    {
        if (e.IdContrato <= 0) throw new ValidacionException("Debe seleccionar un contrato.");
        if (string.IsNullOrWhiteSpace(e.LugarEntrega)) throw new ValidacionException("Debe indicar el lugar de entrega.");
    }
}
