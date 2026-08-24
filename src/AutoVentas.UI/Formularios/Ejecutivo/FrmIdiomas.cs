using AutoVentas.DAL.Repositorios;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>
/// T05. Gestión de Múltiples Idiomas — pantalla para incorporar idiomas nuevos y editar sus
/// leyendas DESDE EL SISTEMA (no hace falta tocar la base de datos a mano). Al guardar, si el
/// idioma editado es el que está activo en ese momento, se vuelve a aplicar automáticamente
/// (GestorIdioma.CambiarIdioma) para reflejar los cambios sin reiniciar la aplicación.
/// </summary>
public partial class FrmIdiomas : Form, IObservadorIdioma
{
    private readonly RepositorioIdiomas _repositorio = new();

    public FrmIdiomas()
    {
        InitializeComponent();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            CargarCombo();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void CmbIdioma_SelectedIndexChanged(object? sender, EventArgs e) => CargarGrilla();

    private void CargarCombo()
    {
        var idiomaAnterior = _cmbIdioma.SelectedItem as Idioma;
        _cmbIdioma.Items.Clear();
        _cmbIdioma.Items.AddRange(_repositorio.ObtenerTodos().Cast<object>().ToArray());

        var aReseleccionar = idiomaAnterior is null
            ? null
            : _cmbIdioma.Items.Cast<Idioma>().FirstOrDefault(i => i.IdIdioma == idiomaAnterior.IdIdioma);

        _cmbIdioma.SelectedItem = aReseleccionar ?? _cmbIdioma.Items.Cast<Idioma>().FirstOrDefault();
    }

    private void CargarGrilla()
    {
        _grilla.Rows.Clear();
        if (_cmbIdioma.SelectedItem is not Idioma idioma) return;

        var traducciones = _repositorio.ObtenerTraducciones(idioma.Codigo);
        var claves = _repositorio.ObtenerClavesConocidas();

        foreach (var clave in claves)
        {
            traducciones.TryGetValue(clave, out var valor);
            _grilla.Rows.Add(clave, valor ?? string.Empty);
        }
    }

    private void BtnNuevoIdioma_Click(object? sender, EventArgs e)
    {
        using var frm = new FrmNuevoIdioma();
        if (frm.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            var idNuevo = _repositorio.AgregarIdioma(frm.Codigo, frm.Nombre);
            GestorIdioma.Instancia.Inicializar(GestorIdioma.Instancia.CodigoIdiomaActual);
            CargarCombo();
            _cmbIdioma.SelectedItem = _cmbIdioma.Items.Cast<Idioma>().FirstOrDefault(i => i.IdIdioma == idNuevo);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbIdioma.SelectedItem is not Idioma idioma) return;

        try
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                var clave = Convert.ToString(fila.Cells["Clave"].Value) ?? string.Empty;
                var valor = Convert.ToString(fila.Cells["Valor"].Value) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(clave)) continue;
                _repositorio.GuardarTraduccion(idioma.IdIdioma, clave, valor);
            }

            // Si el idioma editado es el que está activo, se vuelve a aplicar para que se vean
            // los cambios recién guardados sin tener que reiniciar la aplicación (T05, Observer).
            if (idioma.Codigo == GestorIdioma.Instancia.CodigoIdiomaActual)
            {
                GestorIdioma.Instancia.CambiarIdioma(idioma.Codigo);
            }

            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.idiomaguardado"),
                "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    public void ActualizarIdioma()
    {
        Text = GestorIdioma.Instancia.Traducir("menu.idiomas");
        _btnNuevoIdioma.Text = GestorIdioma.Instancia.Traducir("btn.nuevoidioma");
        _btnGuardar.Text = GestorIdioma.Instancia.Traducir("btn.guardar");
    }
}
