using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Comunes;

/// <summary>
/// Encapsula por COMPOSICIÓN (no por herencia) el comportamiento común de toda gestión CRUD
/// (grilla + alta/edición/baja/refresco), para reutilizarlo entre las gestiones de Vehículos,
/// Clientes, Mantenimientos, Presupuestos, Contratos, Reservas, Pagos, Entregas y Reportes.
/// <para>
/// Se usa composición en lugar de una clase base genérica (como se hacía antes) porque el
/// diseñador visual de Windows Forms no admite clases con una clase base genérica: cualquier
/// <c>Form</c> que herede de <c>Base&lt;T&gt;</c> no puede abrirse en la vista de diseño. Al mover
/// la lógica reutilizable a esta clase auxiliar, cada formulario concreto vuelve a heredar
/// directamente de <see cref="Form"/> y queda compatible con el diseñador.
/// </para>
/// </summary>
public class ControladorListadoCrud<T>
{
    private readonly Form _formulario;
    private readonly DataGridView _grilla;
    private readonly Func<List<T>> _obtenerDatos;
    private readonly Action _abrirAlta;
    private readonly Action<T> _abrirEdicion;
    private readonly Action<T> _eliminar;

    public ControladorListadoCrud(
        Form formulario,
        DataGridView grilla,
        Func<List<T>> obtenerDatos,
        Action abrirAlta,
        Action<T> abrirEdicion,
        Action<T> eliminar)
    {
        _formulario = formulario;
        _grilla = grilla;
        _obtenerDatos = obtenerDatos;
        _abrirAlta = abrirAlta;
        _abrirEdicion = abrirEdicion;
        _eliminar = eliminar;
    }

    public T? ObtenerSeleccionado() =>
        _grilla.SelectedRows.Count == 0 ? default : (T)_grilla.SelectedRows[0].DataBoundItem;

    public void Refrescar()
    {
        try
        {
            _grilla.DataSource = null;
            _grilla.DataSource = _obtenerDatos();
        }
        catch (Exception ex)
        {
            MessageBox.Show(_formulario, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void Nuevo()
    {
        _abrirAlta();
        Refrescar();
    }

    public void Editar()
    {
        if (ObtenerSeleccionado() is { } item)
        {
            _abrirEdicion(item);
            Refrescar();
        }
    }

    public void EliminarSeleccionado()
    {
        if (ObtenerSeleccionado() is not { } item) return;

        var confirmar = MessageBox.Show(_formulario,
            GestorIdioma.Instancia.Traducir("msg.confirmareliminar"),
            GestorIdioma.Instancia.Traducir("btn.eliminar"),
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirmar != DialogResult.Yes) return;

        try
        {
            _eliminar(item);
            Refrescar();
        }
        catch (Exception ex)
        {
            MessageBox.Show(_formulario, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
