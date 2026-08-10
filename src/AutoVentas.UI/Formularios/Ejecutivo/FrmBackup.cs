using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Backup;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>T07. Gestión de Backup: catálogo de copias de seguridad + generación/restauración.</summary>
public class FrmBackup : Form, IObservadorIdioma
{
    private const string NombreBaseDeDatos = "AutoVentasDB";
    private readonly ServicioBackup _servicio = new();

    private readonly DataGridView _grilla = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoGenerateColumns = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly FlowLayoutPanel _panelBotones = new() { Dock = DockStyle.Top, Height = 42, Padding = new Padding(6) };
    private readonly Button _btnGenerar = new() { AutoSize = true };
    private readonly Button _btnRestaurar = new() { AutoSize = true };
    private readonly Button _btnRefrescar = new() { AutoSize = true };

    public FrmBackup()
    {
        Width = 760;
        Height = 480;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroBackup.IdBackup), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroBackup.FechaHora), HeaderText = "Fecha/Hora" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroBackup.RutaArchivo), HeaderText = "Archivo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(RegistroBackup.Resultado), HeaderText = "Resultado" });

        _panelBotones.Controls.AddRange(new Control[] { _btnGenerar, _btnRestaurar, _btnRefrescar });
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _btnGenerar.Click += BtnGenerar_Click;
        _btnRestaurar.Click += BtnRestaurar_Click;
        _btnRefrescar.Click += (_, _) => Refrescar();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void Refrescar()
    {
        _grilla.DataSource = null;
        _grilla.DataSource = _servicio.ObtenerCatalogo();
    }

    private void BtnGenerar_Click(object? sender, EventArgs e)
    {
        using var dialogo = new SaveFileDialog
        {
            Filter = "Backup SQL Server (*.bak)|*.bak",
            FileName = $"{NombreBaseDeDatos}_{DateTime.Now:yyyyMMdd_HHmmss}.bak"
        };

        if (dialogo.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            _servicio.GenerarBackup(dialogo.FileName, NombreBaseDeDatos);
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.backupgenerado"), "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refrescar();
        }
        catch (AutoVentasException ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnRestaurar_Click(object? sender, EventArgs e)
    {
        if (_grilla.SelectedRows.Count == 0) return;
        var seleccionado = (RegistroBackup)_grilla.SelectedRows[0].DataBoundItem;

        var confirmar = MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.confirmarrestaurar"),
            GestorIdioma.Instancia.Traducir("menu.backup"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes) return;

        try
        {
            _servicio.RestaurarBackup(seleccionado.IdBackup, NombreBaseDeDatos);
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.backuprestaurado"), "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (AutoVentasException ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.backup");
        _btnGenerar.Text = t.Traducir("btn.generarbackup");
        _btnRestaurar.Text = t.Traducir("btn.restaurar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
