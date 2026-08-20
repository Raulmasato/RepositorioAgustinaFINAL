using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Bitacora;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>
/// T06b. Control de cambios: permite elegir una tabla y el id de un registro puntual y ver,
/// campo a campo, quién cambió qué y cuándo — reconstruyendo así el historial de esa entidad.
/// </summary>
public class FrmHistorialCambios : Form, IObservadorIdioma
{
    private static readonly string[] TablasControladas =
        RepositorioIntegridad.ObtenerNombresTablas().ToArray();

    private readonly ServicioControlCambios _servicio = new();

    private readonly Label _lblTabla = new() { Left = 10, Top = 14, Width = 45 };
    private readonly ComboBox _cmbTabla = new() { Left = 55, Top = 10, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblId = new() { Left = 225, Top = 14, Width = 25 };
    private readonly NumericUpDown _numId = new() { Left = 255, Top = 10, Width = 80, Minimum = 1, Maximum = int.MaxValue };
    private readonly Button _btnBuscar = new() { Left = 345, Top = 8, Width = 90 };

    private readonly DataGridView _grilla = new()
    {
        Top = 42, Left = 0, Dock = DockStyle.Fill,
        ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoGenerateColumns = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly Panel _panelFiltros = new() { Dock = DockStyle.Top, Height = 42 };

    public FrmHistorialCambios()
    {
        Width = 900;
        Height = 520;
        StartPosition = FormStartPosition.CenterParent;

        _cmbTabla.Items.AddRange(TablasControladas.Cast<object>().ToArray());
        if (_cmbTabla.Items.Count > 0) _cmbTabla.SelectedIndex = 0;

        _panelFiltros.Controls.AddRange(new Control[] { _lblTabla, _cmbTabla, _lblId, _numId, _btnBuscar });
        Controls.Add(_grilla);
        Controls.Add(_panelFiltros);

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroControlCambio.FechaHora), HeaderText = "Fecha/Hora", Width = 140 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroControlCambio.NombreUsuario), HeaderText = "Usuario" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroControlCambio.TipoOperacion), HeaderText = "Operación" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroControlCambio.Campo), HeaderText = "Campo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroControlCambio.ValorAnterior), HeaderText = "Valor anterior" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroControlCambio.ValorNuevo), HeaderText = "Valor nuevo" });

        _btnBuscar.Click += (_, _) => Buscar();

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

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.historialcambios");
        _lblTabla.Text = t.Traducir("lbl.tabla");
        _lblId.Text = "Id";
        _btnBuscar.Text = t.Traducir("btn.buscar");
    }
}
