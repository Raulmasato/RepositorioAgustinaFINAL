using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Bitacora;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>T06a. Consulta de Bitácora, con búsqueda combinada por usuario, actividad y rango de fechas.</summary>
public partial class FrmBitacora : Form, IObservadorIdioma
{
    private readonly ServicioBitacora _servicio = new();

    public FrmBitacora()
    {
        InitializeComponent();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            Buscar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void Buscar()
    {
        var actividad = string.IsNullOrWhiteSpace(_txtActividad.Text) ? null : _txtActividad.Text.Trim();
        _grilla.DataSource = null;
        _grilla.DataSource = _servicio.Buscar(null, actividad, _dtpDesde.Value.Date, _dtpHasta.Value.Date.AddDays(1).AddSeconds(-1));
    }

    private void BtnBuscar_Click(object? sender, EventArgs e) => Buscar();

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.bitacora");
        _lblActividad.Text = t.Traducir("lbl.actividad");
        _lblDesde.Text = t.Traducir("lbl.desde");
        _lblHasta.Text = t.Traducir("lbl.hasta");
        _btnBuscar.Text = t.Traducir("btn.buscar");
    }
}
