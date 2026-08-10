using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Comunes;

/// <summary>
/// Formulario base reutilizado por todas las gestiones CRUD (Vehículos, Clientes, Contratos,
/// Reservas, Pagos, Entregas, Presupuestos, Mantenimientos, Reportes). Concentra la grilla,
/// la barra de botones y el ciclo alta/edición/baja/refresco, de forma que cada gestión
/// concreta solo defina sus columnas y sus formularios de edición (reuso de UI).
/// También se suscribe como observador de idioma (T05) para traducir sus textos en caliente.
/// </summary>
public abstract class FormListadoBase<T> : Form, IObservadorIdioma
{
    protected readonly DataGridView Grilla = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoGenerateColumns = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly FlowLayoutPanel _panelBotones = new() { Dock = DockStyle.Top, Height = 42, Padding = new Padding(6) };
    private readonly Button _btnNuevo = new() { AutoSize = true };
    private readonly Button _btnEditar = new() { AutoSize = true };
    private readonly Button _btnEliminar = new() { AutoSize = true };
    private readonly Button _btnRefrescar = new() { AutoSize = true };

    protected abstract string ClaveTituloIdioma { get; }

    protected FormListadoBase()
    {
        Width = 820;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;

        _panelBotones.Controls.AddRange(new Control[] { _btnNuevo, _btnEditar, _btnEliminar, _btnRefrescar });
        Controls.Add(Grilla);
        Controls.Add(_panelBotones);

        _btnNuevo.Click += (_, _) => { AbrirAlta(); Refrescar(); };
        _btnEditar.Click += (_, _) => { if (ObtenerSeleccionado() is { } item) { AbrirEdicion(item); Refrescar(); } };
        _btnEliminar.Click += (_, _) => { if (ObtenerSeleccionado() is { } item) ConfirmarYEliminar(item); };
        _btnRefrescar.Click += (_, _) => Refrescar();

        ConfigurarColumnas(Grilla);

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    protected abstract void ConfigurarColumnas(DataGridView grilla);
    protected abstract List<T> ObtenerDatos();
    protected abstract void AbrirAlta();
    protected abstract void AbrirEdicion(T seleccionado);
    protected abstract void Eliminar(T seleccionado);

    protected T? ObtenerSeleccionado()
    {
        if (Grilla.SelectedRows.Count == 0) return default;
        return (T)Grilla.SelectedRows[0].DataBoundItem;
    }

    protected void Refrescar()
    {
        try
        {
            Grilla.DataSource = null;
            Grilla.DataSource = ObtenerDatos();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ConfirmarYEliminar(T item)
    {
        var confirmar = MessageBox.Show(this,
            GestorIdioma.Instancia.Traducir("msg.confirmareliminar"),
            GestorIdioma.Instancia.Traducir("btn.eliminar"),
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirmar != DialogResult.Yes) return;

        try
        {
            Eliminar(item);
            Refrescar();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public virtual void ActualizarIdioma()
    {
        Text = GestorIdioma.Instancia.Traducir(ClaveTituloIdioma);
        _btnNuevo.Text = GestorIdioma.Instancia.Traducir("btn.nuevo");
        _btnEditar.Text = GestorIdioma.Instancia.Traducir("btn.editar");
        _btnEliminar.Text = GestorIdioma.Instancia.Traducir("btn.eliminar");
        _btnRefrescar.Text = GestorIdioma.Instancia.Traducir("btn.refrescar");
    }
}
