namespace AutoVentas.DAL.Repositorios;

/// <summary>Contrato CRUD común a todos los repositorios (patrón Repository sobre ADO.NET puro).</summary>
public interface IRepositorio<T>
{
    int Agregar(T entidad);
    void Modificar(T entidad);
    void Eliminar(int id);
    T? ObtenerPorId(int id);
    List<T> ObtenerTodos();
}
