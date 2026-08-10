using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.UI.Formularios.Cliente;

/// <summary>
/// "Crear reserva" desde la óptica del Cliente: solo puede crear y listar sus propias
/// reservas (a diferencia del Ejecutivo, que tiene el CRUD completo sobre todas las reservas).
/// </summary>
public class FrmMisReservas : Form, IObservadorIdioma
{
    private readonly GestorReservas _gestorReservas = new();
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
    private readonly Button _btnNuevaReserva = new() { AutoSize = true };
    private readonly Button _btnRefrescar = new() { AutoSize = true };

    public FrmMisReservas()
    {
        Width = 760;
        Height = 480;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.IdReserva), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.VehiculoDescripcion), HeaderText = "Vehículo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.FechaReserva), HeaderText = "Fecha reserva" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.FechaVencimiento), HeaderText = "Vence" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.Estado), HeaderText = "Estado" });

        _panelBotones.Controls.AddRange(new Control[] { _btnNuevaReserva, _btnRefrescar });
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _btnNuevaReserva.Click += BtnNuevaReserva_Click;
        _btnRefrescar.Click += (_, _) => Refrescar();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private Domain.Entidades.Cliente? ObtenerClienteActual()
    {
        var usuario = SesionActual.Instancia.UsuarioLogueado!;
        return _gestorClientes.ObtenerPorUsuario(usuario.IdUsuario);
    }

    private void Refrescar()
    {
        var cliente = ObtenerClienteActual();
        _grilla.DataSource = null;
        _grilla.DataSource = cliente is null ? new List<Reserva>() : _gestorReservas.ObtenerPorCliente(cliente.IdCliente);
    }

    private void BtnNuevaReserva_Click(object? sender, EventArgs e)
    {
        var cliente = ObtenerClienteActual();
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
        Text = t.Traducir("menu.reservas");
        _btnNuevaReserva.Text = t.Traducir("btn.nuevareserva");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}
