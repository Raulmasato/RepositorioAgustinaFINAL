using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Contratos (Ejecutivo). Puede originarse en un Presupuesto (&lt;&lt;include&gt;&gt;).</summary>
public class FrmContratos : Form, IObservadorIdioma
{
    private readonly GestorContratos _gestor = new();

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
    private readonly ControladorListadoCrud<Contrato> _controlador;

    public FrmContratos()
    {
        Width = 820;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.IdContrato), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.VehiculoDescripcion), HeaderText = "Vehículo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.ClienteNombreCompleto), HeaderText = "Cliente" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.FechaContrato), HeaderText = "Fecha" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.Precio), HeaderText = "Precio" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Contrato.Estado), HeaderText = "Estado" });

        _panelBotones.Controls.AddRange(new Control[] { _btnNuevo, _btnEditar, _btnEliminar, _btnRefrescar });
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _controlador = new ControladorListadoCrud<Contrato>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, c => _gestor.Eliminar(c.IdContrato));

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
        using var frm = new FrmContratoEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Contrato seleccionado)
    {
        using var frm = new FrmContratoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.contratos");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}

internal class FrmContratoEditar : Form, IObservadorIdioma
{
    private readonly GestorContratos _gestor = new();
    private readonly Contrato? _original;

    private readonly Label _lblVehiculo = new() { Left = 20, Top = 20, Width = 100 };
    private readonly ComboBox _cmbVehiculo = new() { Left = 130, Top = 17, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblCliente = new() { Left = 20, Top = 55, Width = 100 };
    private readonly ComboBox _cmbCliente = new() { Left = 130, Top = 52, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblPresupuesto = new() { Left = 20, Top = 90, Width = 100 };
    private readonly ComboBox _cmbPresupuesto = new() { Left = 130, Top = 87, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblPrecio = new() { Left = 20, Top = 125, Width = 100 };
    private readonly NumericUpDown _numPrecio = new() { Left = 130, Top = 122, Width = 150, Maximum = 100_000_000, DecimalPlaces = 2 };
    private readonly Label _lblEstado = new() { Left = 20, Top = 160, Width = 100 };
    private readonly ComboBox _cmbEstado = new() { Left = 130, Top = 157, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 200, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 200, Width = 90 };

    public FrmContratoEditar(Contrato? contrato)
    {
        _original = contrato;
        Width = 400;
        Height = 290;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[]
        {
            _lblVehiculo, _cmbVehiculo, _lblCliente, _cmbCliente, _lblPresupuesto, _cmbPresupuesto,
            _lblPrecio, _numPrecio, _lblEstado, _cmbEstado, _btnGuardar, _btnCancelar
        });

        _cmbEstado.Items.AddRange(Enum.GetValues<EstadoContrato>().Cast<object>().ToArray());
        _numPrecio.Value = contrato is not null ? Math.Clamp(contrato.Precio, _numPrecio.Minimum, _numPrecio.Maximum) : 0;
        _cmbEstado.SelectedItem = contrato?.Estado ?? EstadoContrato.Vigente;

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);

        // Diferido a Load: el diseñador de Visual Studio no debe ejecutar consultas a la BD
        // al instanciar este formulario para dibujarlo.
        Load += (_, _) =>
        {
            CargarCombosDependientesDeBD();
            ActualizarIdioma();
        };
    }

    private void CargarCombosDependientesDeBD()
    {
        _cmbVehiculo.Items.AddRange(new GestorVehiculos().ObtenerTodos().Cast<object>().ToArray());
        _cmbCliente.Items.AddRange(new GestorClientes().ObtenerTodos().Cast<object>().ToArray());
        _cmbPresupuesto.Items.Add("(Ninguno)");
        _cmbPresupuesto.Items.AddRange(new GestorPresupuestos().ObtenerTodos().Cast<object>().ToArray());

        if (_original is not null)
        {
            _cmbVehiculo.SelectedItem = _cmbVehiculo.Items.Cast<Vehiculo>().FirstOrDefault(v => v.IdVehiculo == _original.IdVehiculo);
            _cmbCliente.SelectedItem = _cmbCliente.Items.Cast<Cliente>().FirstOrDefault(c => c.IdCliente == _original.IdCliente);
            _cmbPresupuesto.SelectedItem = _original.IdPresupuesto is int idP
                ? _cmbPresupuesto.Items.OfType<Presupuesto>().FirstOrDefault(p => p.IdPresupuesto == idP)
                : _cmbPresupuesto.Items[0];
        }
        else
        {
            _cmbPresupuesto.SelectedIndex = 0;
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbVehiculo.SelectedItem is not Vehiculo vehiculo || _cmbCliente.SelectedItem is not Cliente cliente)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.seleccionevehiculocliente"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var usuario = SesionActual.Instancia.UsuarioLogueado!;
            var contrato = _original ?? new Contrato { FechaContrato = DateTime.Now, IdUsuarioEjecutivo = usuario.IdUsuario };
            contrato.IdVehiculo = vehiculo.IdVehiculo;
            contrato.IdCliente = cliente.IdCliente;
            contrato.IdPresupuesto = _cmbPresupuesto.SelectedItem is Presupuesto presupuesto ? presupuesto.IdPresupuesto : null;
            contrato.Precio = _numPrecio.Value;
            contrato.Estado = (EstadoContrato)_cmbEstado.SelectedItem!;

            if (_original is null) _gestor.Agregar(contrato);
            else _gestor.Modificar(contrato);

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
        Text = t.Traducir("menu.contratos");
        _lblVehiculo.Text = t.Traducir("lbl.vehiculo");
        _lblCliente.Text = t.Traducir("lbl.cliente");
        _lblPresupuesto.Text = t.Traducir("menu.presupuestos");
        _lblPrecio.Text = t.Traducir("lbl.precio");
        _lblEstado.Text = t.Traducir("lbl.estado");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
