using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Bitacora;
using AutoVentas.Services.Integridad;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.BLL;

/// <summary>
/// T02. Gestión de Log In / Log Out del sistema.
/// Verifica identidad (usuario + clave), asigna el perfil (rol) correspondiente y deja
/// constancia en la Sesión (Singleton) y en la Bitácora. También resuelve el alta de
/// nuevos usuarios (formulario de Registro).
/// </summary>
public class GestorAutenticacion
{
    private readonly RepositorioUsuarios _repositorioUsuarios = new();
    private readonly RepositorioRoles _repositorioRoles = new();
    private readonly RepositorioClientes _repositorioClientes = new();
    private readonly ServicioBitacora _bitacora = new();
    private readonly ServicioDigitoVerificador _digitoVerificador = new();

    /// <summary>Verifica usuario/clave e inicia sesión (patrón Singleton en <see cref="SesionActual"/>).</summary>
    public Usuario IniciarSesion(string nombreUsuario, string claveEnClaro)
    {
        var usuario = _repositorioUsuarios.ObtenerPorNombreUsuario(nombreUsuario)
            ?? throw new SeguridadException("Usuario o contraseña incorrectos.");

        if (!usuario.Activo)
        {
            throw new SeguridadException("El usuario se encuentra deshabilitado.");
        }

        if (!ServicioCriptografia.VerificarClave(claveEnClaro, usuario.ClaveHash, usuario.ClaveSalt))
        {
            _bitacora.Registrar("Intento de login fallido", $"Usuario={nombreUsuario}");
            throw new SeguridadException("Usuario o contraseña incorrectos.");
        }

        SesionActual.Instancia.IniciarSesion(usuario);
        _bitacora.Registrar("Login", $"Usuario={usuario.NombreUsuario}");
        return usuario;
    }

    public void CerrarSesion()
    {
        var nombreUsuario = SesionActual.Instancia.UsuarioLogueado?.NombreUsuario;
        _bitacora.Registrar("Logout", $"Usuario={nombreUsuario}");
        SesionActual.Instancia.CerrarSesion();
    }

    /// <summary>Registra un nuevo usuario. Si el rol elegido es Cliente, crea también el
    /// registro de Cliente asociado (nombre, apellido y DNI) para poder operar reservas.</summary>
    public Usuario Registrar(string nombreUsuario, string claveEnClaro, NombreRol rol,
        string? nombreCliente = null, string? apellidoCliente = null, string? dniCliente = null)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || nombreUsuario.Trim().Length < 4)
            throw new ValidacionException("El nombre de usuario debe tener al menos 4 caracteres.");

        if (string.IsNullOrWhiteSpace(claveEnClaro) || claveEnClaro.Length < 6)
            throw new ValidacionException("La contraseña debe tener al menos 6 caracteres.");

        if (_repositorioUsuarios.ExisteNombreUsuario(nombreUsuario))
            throw new ValidacionException("Ya existe un usuario con ese nombre.");

        var rolBd = _repositorioRoles.ObtenerPorNombre(rol.ToString())
            ?? throw new ValidacionException("El rol seleccionado no es válido.");

        var (hash, salt) = ServicioCriptografia.HashClave(claveEnClaro);

        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuario.Trim(),
            ClaveHash = hash,
            ClaveSalt = salt,
            IdRol = rolBd.IdRol,
            Rol = rol,
            Activo = true
        };

        usuario.IdUsuario = _repositorioUsuarios.Agregar(usuario);
        _digitoVerificador.RecalcularYGuardar("Usuarios");

        if (rol == NombreRol.Cliente)
        {
            if (string.IsNullOrWhiteSpace(nombreCliente) || string.IsNullOrWhiteSpace(apellidoCliente) || string.IsNullOrWhiteSpace(dniCliente))
                throw new ValidacionException("Para registrarse como Cliente debe completar nombre, apellido y DNI.");

            var cliente = new Cliente
            {
                Nombre = nombreCliente.Trim(),
                Apellido = apellidoCliente.Trim(),
                DniEncriptado = ServicioCriptografia.Encriptar(dniCliente.Trim()),
                IdUsuario = usuario.IdUsuario
            };
            _repositorioClientes.Agregar(cliente);
            _digitoVerificador.RecalcularYGuardar("Clientes");
        }

        _bitacora.Registrar("Registro de usuario", $"Usuario={usuario.NombreUsuario}, Rol={rol}");
        return usuario;
    }
}
