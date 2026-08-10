using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;
using AutoVentas.Services.Seguridad;
using AutoVentas.UI.Formularios.Comunes;

namespace AutoVentas.UI.Formularios.Ejecutivo;

/// <summary>Gestión de Pagos (Ejecutivo).</summary>
public class FrmPagos : Form, IObservadorIdioma
{
    private readonly GestorPagos _gestor = new();

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
    private readonly ControladorListadoCrud<Pago> _controlador;

    public FrmPagos()
    {
        Width = 820;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Pago.IdPago), HeaderText = "Id", Width = 50 });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Pago.ContratoDescripcion), HeaderText = "Contrato" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Pago.Monto), HeaderText = "Monto" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Pago.FechaPago), HeaderText = "Fecha" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Pago.MetodoPago), HeaderText = "Método" });

        _panelBotones.Controls.AddRange(new Control[] { _btnNuevo, _btnEditar, _btnEliminar, _btnRefrescar });
        Controls.Add(_grilla);
        Controls.Add(_panelBotones);

        _controlador = new ControladorListadoCrud<Pago>(
            this, _grilla, () => _gestor.ObtenerTodos(),
            AbrirAlta, AbrirEdicion, p => _gestor.Eliminar(p.IdPago));

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
        using var frm = new FrmPagoEditar(null);
        frm.ShowDialog(this);
    }

    private void AbrirEdicion(Pago seleccionado)
    {
        using var frm = new FrmPagoEditar(seleccionado);
        frm.ShowDialog(this);
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("menu.pagos");
        _btnNuevo.Text = t.Traducir("btn.nuevo");
        _btnEditar.Text = t.Traducir("btn.editar");
        _btnEliminar.Text = t.Traducir("btn.eliminar");
        _btnRefrescar.Text = t.Traducir("btn.refrescar");
    }
}

internal class FrmPagoEditar : Form, IObservadorIdioma
{
    private static readonly string[] MetodosPago = { "Efectivo", "Transferencia", "Tarjeta de crédito", "Tarjeta de débito", "Cheque" };

    private readonly GestorPagos _gestor = new();
    private readonly Pago? _original;

    private readonly Label _lblContrato = new() { Left = 20, Top = 20, Width = 100 };
    private readonly ComboBox _cmbContrato = new() { Left = 130, Top = 17, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblMonto = new() { Left = 20, Top = 55, Width = 100 };
    private readonly NumericUpDown _numMonto = new() { Left = 130, Top = 52, Width = 150, Maximum = 100_000_000, DecimalPlaces = 2 };
    private readonly Label _lblMetodo = new() { Left = 20, Top = 90, Width = 100 };
    private readonly ComboBox _cmbMetodo = new() { Left = 130, Top = 87, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Button _btnGuardar = new() { Left = 130, Top = 130, Width = 90 };
    private readonly Button _btnCancelar = new() { Left = 230, Top = 130, Width = 90 };

    public FrmPagoEditar(Pago? pago)
    {
        _original = pago;
        Width = 400;
        Height = 220;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[] { _lblContrato, _cmbContrato, _lblMonto, _numMonto, _lblMetodo, _cmbMetodo, _btnGuardar, _btnCancelar });

        _cmbMetodo.Items.AddRange(MetodosPago);

        if (pago is not null)
        {
            _numMonto.Value = Math.Clamp(pago.Monto, _numMonto.Minimum, _numMonto.Maximum);
            _cmbMetodo.SelectedItem = pago.MetodoPago;
        }

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
        _cmbContrato.Items.AddRange(new GestorContratos().ObtenerTodos().Cast<object>().ToArray());

        if (_original is not null)
        {
            _cmbContrato.SelectedItem = _cmbContrato.Items.Cast<Contrato>().FirstOrDefault(c => c.IdContrato == _original.IdContrato);
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbContrato.SelectedItem is not Contrato contrato || _cmbMetodo.SelectedItem is not string metodo)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.completetodosloscampos"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var usuario = SesionActual.Instancia.UsuarioLogueado!;
            var pago = _original ?? new Pago { FechaPago = DateTime.Now, IdUsuarioEjecutivo = usuario.IdUsuario };
            pago.IdContrato = contrato.IdContrato;
            pago.Monto = _numMonto.Value;
            pago.MetodoPago = metodo;

            if (_original is null) _gestor.Agregar(pago);
            else _gestor.Modificar(pago);

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
        Text = t.Traducir("menu.pagos");
        _lblContrato.Text = t.Traducir("menu.contratos");
        _lblMonto.Text = t.Traducir("lbl.monto");
        _lblMetodo.Text = t.Traducir("lbl.metodopago");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
