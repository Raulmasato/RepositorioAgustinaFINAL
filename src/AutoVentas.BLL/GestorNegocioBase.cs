using AutoVentas.DAL.Repositorios;
using AutoVentas.Services.Bitacora;
using AutoVentas.Services.Integridad;

namespace AutoVentas.BLL;

/// <summary>
/// Base común para toda gestión CRUD de negocio (Vehículos, Clientes, Contratos, etc.).
/// Concentra en un único lugar el flujo que se repite en cada una de las gestiones:
/// validar -> persistir -> auditar (control de cambios) -> recalcular dígito verificador -> bitácora.
/// Las clases concretas solo aportan las reglas de validación propias de la entidad y,
/// si corresponde, consultas adicionales (reuso entre gestiones, criterio evaluado en la cátedra).
/// </summary>
public abstract class GestorNegocioBase<T>
{
    protected readonly IRepositorio<T> Repositorio;
    protected readonly string NombreTabla;

    private readonly ServicioControlCambios _controlCambios = new();
    private readonly ServicioDigitoVerificador _digitoVerificador = new();
    private readonly ServicioBitacora _bitacora = new();

    protected GestorNegocioBase(IRepositorio<T> repositorio, string nombreTabla)
    {
        Repositorio = repositorio;
        NombreTabla = nombreTabla;
    }

    protected abstract int ObtenerId(T entidad);

    /// <summary>Lanza <see cref="AutoVentas.Domain.Excepciones.ValidacionException"/> si la entidad no cumple las reglas de negocio.</summary>
    protected abstract void Validar(T entidad);

    public virtual List<T> ObtenerTodos() => Repositorio.ObtenerTodos();

    public virtual T? ObtenerPorId(int id) => Repositorio.ObtenerPorId(id);

    public virtual int Agregar(T entidad)
    {
        Validar(entidad);
        var id = Repositorio.Agregar(entidad);
        _controlCambios.RegistrarAlta(NombreTabla, id, entidad);
        _digitoVerificador.RecalcularYGuardar(NombreTabla);
        _bitacora.Registrar($"Alta de {NombreTabla}", $"Id={id}");
        return id;
    }

    public virtual void Modificar(T entidadNueva)
    {
        Validar(entidadNueva);
        var id = ObtenerId(entidadNueva);
        var anterior = Repositorio.ObtenerPorId(id);

        Repositorio.Modificar(entidadNueva);

        if (anterior is not null)
        {
            _controlCambios.RegistrarModificacion(NombreTabla, id, anterior, entidadNueva);
        }
        _digitoVerificador.RecalcularYGuardar(NombreTabla);
        _bitacora.Registrar($"Modificación de {NombreTabla}", $"Id={id}");
    }

    public virtual void Eliminar(int id)
    {
        var entidad = Repositorio.ObtenerPorId(id);

        Repositorio.Eliminar(id);

        if (entidad is not null)
        {
            _controlCambios.RegistrarBaja(NombreTabla, id, entidad);
        }
        _digitoVerificador.RecalcularYGuardar(NombreTabla);
        _bitacora.Registrar($"Baja de {NombreTabla}", $"Id={id}");
    }
}
