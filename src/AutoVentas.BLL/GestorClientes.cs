using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.BLL;

/// <summary>Gestión de Clientes (Vendedor). El DNI se guarda encriptado (T03) por ser un dato sensible.</summary>
public class GestorClientes : GestorNegocioBase<Cliente>
{
    public GestorClientes() : base(new RepositorioClientes(), "Clientes") { }

    protected override int ObtenerId(Cliente entidad) => entidad.IdCliente;

    protected override void Validar(Cliente c)
    {
        if (string.IsNullOrWhiteSpace(c.Nombre)) throw new ValidacionException("El nombre del cliente es obligatorio.");
        if (string.IsNullOrWhiteSpace(c.Apellido)) throw new ValidacionException("El apellido del cliente es obligatorio.");
        if (string.IsNullOrWhiteSpace(c.DniPlano) || c.DniPlano.Trim().Length < 6)
            throw new ValidacionException("El DNI ingresado no es válido.");
    }

    public override int Agregar(Cliente c)
    {
        c.DniEncriptado = ServicioCriptografia.Encriptar(c.DniPlano.Trim());
        return base.Agregar(c);
    }

    public override void Modificar(Cliente c)
    {
        c.DniEncriptado = ServicioCriptografia.Encriptar(c.DniPlano.Trim());
        base.Modificar(c);
    }

    /// <summary>Devuelve la lista de clientes con el DNI desencriptado, listo para mostrar en pantalla.</summary>
    public override List<Cliente> ObtenerTodos()
    {
        var clientes = base.ObtenerTodos();
        foreach (var c in clientes)
        {
            c.DniPlano = ServicioCriptografia.Desencriptar(c.DniEncriptado);
        }
        return clientes;
    }

    public override Cliente? ObtenerPorId(int id)
    {
        var cliente = base.ObtenerPorId(id);
        if (cliente is not null)
        {
            cliente.DniPlano = ServicioCriptografia.Desencriptar(cliente.DniEncriptado);
        }
        return cliente;
    }

    public Cliente? ObtenerPorUsuario(int idUsuario)
    {
        var cliente = ((RepositorioClientes)Repositorio).ObtenerPorUsuario(idUsuario);
        if (cliente is not null)
        {
            cliente.DniPlano = ServicioCriptografia.Desencriptar(cliente.DniEncriptado);
        }
        return cliente;
    }
}
