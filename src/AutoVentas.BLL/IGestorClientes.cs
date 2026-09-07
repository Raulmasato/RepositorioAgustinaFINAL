using AutoVentas.Domain.Entidades;

namespace AutoVentas.BLL;

public interface IGestorClientes : IGestorNegocioBase<Cliente>
{
    Cliente? ObtenerPorUsuario(int idUsuario);
}
