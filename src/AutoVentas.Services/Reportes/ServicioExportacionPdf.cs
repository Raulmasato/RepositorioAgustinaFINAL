using AutoVentas.Domain.Entidades;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace AutoVentas.Services.Reportes;

/// <summary>
/// A02. Informe y exportación en PDF.
/// Genera un archivo PDF real a partir de un <see cref="Reporte"/> usando PDFsharp (librería de
/// terceros, MIT). No se utiliza ninguna impresora virtual: el documento se arma directamente,
/// línea por línea, con la API de dibujo de PDFsharp (XGraphics) y se guarda en disco.
/// </summary>
public class ServicioExportacionPdf
{
    private static bool _fuentesConfiguradas;

    public ServicioExportacionPdf()
    {
        // PDFsharp 6 no lee las fuentes del sistema operativo por defecto; en Windows (donde
        // corre esta aplicación) se habilita para poder usar "Arial"/"Consolas" directamente.
        if (!_fuentesConfiguradas)
        {
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            _fuentesConfiguradas = true;
        }
    }

    /// <summary>Renderiza el reporte (título, metadatos y contenido) a un archivo PDF en <paramref name="rutaDestino"/>.</summary>
    public void ExportarReporte(Reporte reporte, string rutaDestino)
    {
        var documento = new PdfDocument();
        documento.Info.Title = reporte.Titulo;
        documento.Info.Author = "AutoVentas";
        documento.Info.Subject = reporte.TipoReporte.ToString();

        var fuenteTitulo = new XFont("Arial", 18, XFontStyleEx.Bold);
        var fuenteSubtitulo = new XFont("Arial", 11, XFontStyleEx.Italic);
        var fuenteTexto = new XFont("Consolas", 9, XFontStyleEx.Regular);

        const double margen = 40;

        var pagina = documento.AddPage();
        var gfx = XGraphics.FromPdfPage(pagina);
        double y = margen;

        gfx.DrawString(reporte.Titulo, fuenteTitulo, XBrushes.Black, new XPoint(margen, y));
        y += 28;

        gfx.DrawString(
            $"Tipo: {reporte.TipoReporte}   |   Período: {reporte.FechaDesde:d} a {reporte.FechaHasta:d}   |   Generado: {reporte.FechaGeneracion:g}",
            fuenteSubtitulo, XBrushes.DarkSlateGray, new XPoint(margen, y));
        y += 20;

        gfx.DrawLine(XPens.Gray, margen, y, pagina.Width.Point - margen, y);
        y += 16;

        foreach (var linea in (reporte.Contenido ?? string.Empty).Split('\n'))
        {
            if (y > pagina.Height.Point - margen)
            {
                pagina = documento.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                y = margen;
            }

            gfx.DrawString(linea.TrimEnd('\r'), fuenteTexto, XBrushes.Black, new XPoint(margen, y));
            y += 14;
        }

        documento.Save(rutaDestino);
    }
}
