using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Reportes;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Reportes (Ejecutivo). El contenido se genera automáticamente a partir
/// del tipo de reporte y el rango de fechas seleccionado.</summary>
public partial class FrmReportes : Form, IObservadorIdioma
{
    private readonly GestorReportes _gestor = new();
    private readonly ServicioExportacionPdf _servicioPdf = new();
    private readonly ControladorListadoCrud<Reporte> _controlador;

    public FrmReportes()
    {
        InitializeComponent();

        _controlador = new ControladorListadoCrud<Reporte>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, r => _gestor.Eliminar(r.IdReporte));

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            _controlador.Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void BtnNuevo_Click(object? sender, EventArgs e) => _controlador.Nuevo();

    private void BtnEditar_Click(object? sender, EventArgs e) => _controlador.Editar();

    private void BtnEliminar_Click(object? sender, EventArgs e) => _controlador.EliminarSeleccionado();

    private void BtnRefrescar_Click(object? sender, EventArgs e) => _controlador.Refrescar();

    private void AbrirAlta()
    {
        using var frm = new FrmReporteEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Reporte seleccionado)
    {
        using var frm = new FrmReporteEditar(seleccionado);
        frm.ShowDialog(this);
    }

    /// <summary>
    /// A02. Exporta el reporte seleccionado a un archivo PDF real (librería PDFsharp),
    /// eligiendo la ubicación con el diálogo estándar de Windows. No usa impresora virtual.
    /// </summary>
    private void BtnExportarPdf_Click(object? sender, EventArgs e)
    {
        if (_controlador.ObtenerSeleccionado() is not { } reporte)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.seleccionereporte"),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dialogo = new SaveFileDialog
        {
            Filter = "Documento PDF (*.pdf)|*.pdf",
            FileName = $"Reporte_{reporte.IdReporte}_{reporte.Titulo}.pdf"
        };

        if (dialogo.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            _servicioPdf.ExportarReporte(reporte, dialogo.FileName);
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.pdfgenerado"),
                "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.reportes");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
        _btnExportarPdf.Text = t.Traducir("btn.exportarpdf");
    }
}
