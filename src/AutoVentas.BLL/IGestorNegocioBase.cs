namespace AutoVentas.BLL;

/// <summary>Contrato CRUD común a toda gestión de negocio, equivalente en la capa BLL al
/// <c>IRepositorio&lt;T&gt;</c> de la DAL. Permite que la UI (o cualquier otra capa) dependa de
/// la abstracción de cada gestión en lugar de su implementación concreta.</summary>
public interface IGestorNegocioBase<T>
{
    List<T> ObtenerTodos();
    T? ObtenerPorId(int id);
    int Agregar(T entidad);
    void Modificar(T entidad);
    void Eliminar(int id);
}
