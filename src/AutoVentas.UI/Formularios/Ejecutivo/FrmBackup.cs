using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Backup;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>T07. Gestión de Backup: catálogo de copias de seguridad + generación/restauración.</summary>
public partial class FrmBackup : Form, IObservadorIdioma
{
    private const string NombreBaseDeDatos = "AutoVentasDB";
    private readonly ServicioBackup _servicio = new();

    public FrmBackup()
    {
        InitializeComponent();

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

    private void BtnRefrescar_Click(object? sender, EventArgs e) => Refrescar();

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.backup");
        _btnGenerar.Text = t.Traducir("btn.generarbackup");
        _btnRestaurar.Text = t.Traducir("btn.restaurar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
