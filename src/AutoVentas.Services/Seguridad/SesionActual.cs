using AutoVentas.Domain.Entidades;

namespace AutoVentas.Services.Seguridad;

/// <summary>
/// T02. Gestión de Log In / Log Out — patrón Singleton.
/// Mantiene, durante todo el ciclo de vida del proceso, cuál es el único usuario logueado
/// y su perfil (rol) asignado. Todas las capas superiores consultan esta única instancia
/// para saber "quién está usando el sistema ahora".
/// </summary>
public sealed class SesionActual
{
    private static readonly Lazy<SesionActual> InstanciaPerezosa = new(() => new SesionActual());

    public static SesionActual Instancia => InstanciaPerezosa.Value;

    private SesionActual() { }

    public Usuario? UsuarioLogueado { get; private set; }
    public DateTime? FechaHoraLogin { get; private set; }
    public bool HaySesionActiva => UsuarioLogueado is not null;

    public void IniciarSesion(Usuario usuario)
    {
        UsuarioLogueado = usuario;
        FechaHoraLogin = DateTime.Now;
    }

    public void CerrarSesion()
    {
        UsuarioLogueado = null;
        FechaHoraLogin = null;
    }

    public bool TieneRol(NombreRol rol) => UsuarioLogueado?.Rol == rol;
}
