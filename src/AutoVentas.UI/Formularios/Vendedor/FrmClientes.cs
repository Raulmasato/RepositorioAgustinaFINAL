using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Vendedor;

/// <summary>Gestión de Clientes (a cargo del Vendedor).</summary>
public class FrmClientes : Form, IObservadorIdioma
{
    private readonly GestorClientes _gestor = new();

    private readonly DataGridView _grilla = new()
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
    private readonly ControladorListadoCrud<Cliente> _controlador;

    public FrmClientes()
    {
        Width = 820;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Cliente.IdCliente), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Cliente.Nombre), HeaderText = "Nombre" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Cliente.Apellido), HeaderText = "Apellido" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Cliente.DniPlano), HeaderText = "DNI" });

        _panelBotones.Controls.AddRange(new Control[] { _btnNuevo, _btnEditar, _btnEliminar, _btnRefrescar });
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _controlador = new ControladorListadoCrud<Cliente>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, c => _gestor.Eliminar(c.IdCliente));

        _btnNuevo.Click += (_, _) => _controlador.Nuevo();
        _btnEditar.Click += (_, _) => _controlador.Editar();
        _btnEliminar.Click += (_, _) => _controlador.EliminarSeleccionado();
        _btnRefrescar.Click += (_, _) => _controlador.Refrescar();

        Load += (_, _) =>
        {
            GestorIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
            _controlador.Refrescar();
        };
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
    }

    private void AbrirAlta()
    {
        using var frm = new FrmClienteEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Cliente seleccionado)
    {
        using var frm = new FrmClienteEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.clientes");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}

internal class FrmClienteEditar : Form, IObservadorIdioma
{
    private readonly GestorClientes _gestor = new();
    private readonly Cliente? _original;

    private readonly Label _lblNombre = new() { Left = 20, Top = 20, Width = 100 };
    private readonly TextBox _txtNombre = new() { Left = 130, Top = 17, Width = 200 };
    private readonly Label _lblApellido = new() { Left = 20, Top = 55, Width = 100 };
    private readonly TextBox _txtApellido = new() { Left = 130, Top = 52, Width = 200 };
    private readonly Label _lblDni = new() { Left = 20, Top = 90, Width = 100 };
    private readonly TextBox _txtDni = new() { Left = 130, Top = 87, Width = 200 };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 130, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 130, Width = 90 };

    public FrmClienteEditar(Cliente? cliente)
    {
        _original = cliente;
        Width = 380;
        Height = 220;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[] { _lblNombre, _txtNombre, _lblApellido, _txtApellido, _lblDni, _txtDni, _btnGuardar, _btnCancelar });

        if (cliente is not null)
        {
            _txtNombre.Text = cliente.Nombre;
            _txtApellido.Text = cliente.Apellido;
            _txtDni.Text = cliente.DniPlano;
        }

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            var cliente = _original ?? new Cliente();
            cliente.Nombre = _txtNombre.Text.Trim();
            cliente.Apellido = _txtApellido.Text.Trim();
            cliente.DniPlano = _txtDni.Text.Trim();

            if (_original is null) _gestor.Agregar(cliente);
            else _gestor.Modificar(cliente);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.clientes");
        _lblNombre.Text = t.Traducir("lbl.nombre");
        _lblApellido.Text = t.Traducir("lbl.apellido");
        _lblDni.Text = t.Traducir("lbl.dni");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
