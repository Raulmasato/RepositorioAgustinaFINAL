using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Bitacora;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>T06a. Consulta de Bitácora, con búsqueda combinada por usuario, actividad y rango de fechas.</summary>
public class FrmBitacora : Form, IObservadorIdioma
{
    private readonly ServicioBitacora _servicio = new();

    private readonly Label _lblActividad = new() { Left = 10, Top = 14, Width = 70 };
    private readonly TextBox _txtActividad = new() { Left = 85, Top = 10, Width = 180 };
    private readonly Label _lblDesde = new() { Left = 275, Top = 14, Width = 45 };
    private readonly DateTimePicker _dtpDesde = new() { Left = 320, Top = 10, Width = 130, Value = DateTime.Now.AddMonths(-1) };
    private readonly Label _lblHasta = new() { Left = 460, Top = 14, Width = 40 };
    private readonly DateTimePicker _dtpHasta = new() { Left = 500, Top = 10, Width = 130 };
    private readonly Button _btnBuscar = new() { Left = 640, Top = 8, Width = 90 };

    private readonly DataGridView _grilla = new()
    {
        Top = 42, Left = 0, Dock = DockStyle.Fill,
        ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoGenerateColumns = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly Panel _panelFiltros = new() { Dock = DockStyle.Top, Height = 42 };

    public FrmBitacora()
    {
        Width = 900;
        Height = 520;
        StartPosition = FormStartPosition.CenterParent;

        _panelFiltros.Controls.AddRange(new Control[] { _lblActividad, _txtActividad, _lblDesde, _dtpDesde, _lblHasta, _dtpHasta, _btnBuscar });
        Controls.Add(_grilla);
        Controls.Add(_panelFiltros);

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroBitacora.FechaHora), HeaderText = "Fecha/Hora", Width = 140 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroBitacora.NombreUsuario), HeaderText = "Usuario" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroBitacora.Actividad), HeaderText = "Actividad" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroBitacora.Informacion), HeaderText = "Información" });

        _btnBuscar.Click += (_, _) => Buscar();

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
