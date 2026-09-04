using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Bitacora;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>
/// T06b. Control de cambios: permite elegir una tabla y el id de un registro puntual y ver,
/// campo a campo, quién cambió qué y cuándo — reconstruyendo así el historial de esa entidad.
/// </summary>
public partial class FrmHistorialCambios : Form, IObservadorIdioma
{
    private static readonly string[] TablasControladas =
        RepositorioIntegridad.ObtenerNombresTablas().ToArray();

    private readonly ServicioControlCambios _servicio = new();

    public FrmHistorialCambios()
    {
        InitializeComponent();

        _cmbTabla.Items.AddRange(TablasControladas.Cast<object>().ToArray());
        if (_cmbTabla.Items.Count > 0) _cmbTabla.SelectedIndex = 0;

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void Buscar()
    {
        if (_cmbTabla.SelectedItem is not string tabla) return;

        _grilla.DataSource = null;
        _grilla.DataSource = _servicio.ObtenerHistorial(tabla, (int)_numId.Value);
    }

    private void BtnBuscar_Click(object? sender, EventArgs e) => Buscar();

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.historialcambios");
        _lblTabla.Text = t.Traducir("lbl.tabla");
        _lblId.Text = "Id";
        _btnBuscar.Text = t.Traducir("btn.buscar");
    }
}
