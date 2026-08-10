using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.Cliente;

/// <summary>Catálogo de vehículos disponibles, de solo lectura, visible para el rol Cliente.
/// Desde acá el cliente puede iniciar una reserva sobre el vehículo seleccionado.</summary>
public class FrmCatalogoVehiculos : Form, IObservadorIdioma
{
    private readonly GestorVehiculos _gestorVehiculos = new();
    private readonly GestorClientes _gestorClientes = new();

    private readonly DataGridView _grilla = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoGenerateColumns = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly FlowLayoutPanel _panelBotones = new() { Dock = DockStyle.Top, Height = 42, Padding = new Padding(6) };
    private readonly Button _btnReservar = new() { AutoSize = true };

    public FrmCatalogoVehiculos()
    {
        Width = 760;
        Height = 480;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.IdVehiculo), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Marca), HeaderText = "Marca" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Modelo), HeaderText = "Modelo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Color), HeaderText = "Color" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Anio), HeaderText = "Año" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Vehiculo.Precio), HeaderText = "Precio" });

        _panelBotones.Controls.Add(_btnReservar);
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _btnReservar.Click += BtnReservar_Click;

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
        _grilla.DataSource = _gestorVehiculos.ObtenerDisponibles();
    }

    private void BtnReservar_Click(object? sender, EventArgs e)
    {
        if (_grilla.SelectedRows.Count == 0) return;

        var usuario = SesionActual.Instancia.UsuarioLogueado!;
        var cliente = _gestorClientes.ObtenerPorUsuario(usuario.IdUsuario);
        if (cliente is null)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.clientenoencontrado"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var frm = new Ejecutivo.FrmReservaEditar(null, cliente);
        if (frm.ShowDialog(this) == DialogResult.OK)
        {
            Refrescar();
        }
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.vehiculos");
        _btnReservar.Text = t.Traducir("btn.reservar");
    }
}
