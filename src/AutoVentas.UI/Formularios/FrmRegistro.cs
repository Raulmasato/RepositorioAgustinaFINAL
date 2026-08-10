using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios;

/// <summary>Formulario de registro de un nuevo usuario. Si el rol elegido es Cliente,
/// solicita también nombre, apellido y DNI para crear el registro de Cliente asociado.</summary>
public class FrmRegistro : Form, IObservadorIdioma
{
    private readonly Label _lblUsuario = new() { Left = 20, Top = 20, Width = 120 };
    private readonly TextBox _txtUsuario = new() { Left = 150, Top = 17, Width = 220 };

    private readonly Label _lblClave = new() { Left = 20, Top = 55, Width = 120 };
    private readonly TextBox _txtClave = new() { Left = 150, Top = 52, Width = 220, UseSystemPasswordChar = true };

    private readonly Label _lblConfirmarClave = new() { Left = 20, Top = 90, Width = 120 };
    private readonly TextBox _txtConfirmarClave = new() { Left = 150, Top = 87, Width = 220, UseSystemPasswordChar = true };

    private readonly Label _lblRol = new() { Left = 20, Top = 125, Width = 120 };
    private readonly ComboBox _cmbRol = new() { Left = 150, Top = 122, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

    private readonly Label _lblNombre = new() { Left = 20, Top = 160, Width = 120 };
    private readonly TextBox _txtNombre = new() { Left = 150, Top = 157, Width = 220 };

    private readonly Label _lblApellido = new() { Left = 20, Top = 195, Width = 120 };
    private readonly TextBox _txtApellido = new() { Left = 150, Top = 192, Width = 220 };

    private readonly Label _lblDni = new() { Left = 20, Top = 230, Width = 120 };
    private readonly TextBox _txtDni = new() { Left = 150, Top = 227, Width = 220 };

    private readonly Button _btnGuardar = new() { Left = 150, Top = 270, Width = 100 };
    private readonly Button _btnCancelar = new() { Left = 270, Top = 270, Width = 100 };

    private readonly GestorAutenticacion _gestorAutenticacion = new();

    public FrmRegistro()
    {
        Width = 420;
        Height = 360;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Controls.AddRange(new Control[]
        {
            _lblUsuario, _txtUsuario, _lblClave, _txtClave, _lblConfirmarClave, _txtConfirmarClave,
            _lblRol, _cmbRol, _lblNombre, _txtNombre, _lblApellido, _txtApellido, _lblDni, _txtDni,
            _btnGuardar, _btnCancelar
        });

        _cmbRol.Items.AddRange(new object[] { NombreRol.Cliente, NombreRol.Vendedor, NombreRol.Tecnico, NombreRol.Ejecutivo });
        _cmbRol.SelectedIndexChanged += (_, _) => ActualizarVisibilidadCampoCliente();
        _cmbRol.SelectedIndex = 0;

        _btnGuardar.Click += BtnGuardar_Click;
        _btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
        ActualizarVisibilidadCampoCliente();
    }

    private void ActualizarVisibilidadCampoCliente()
    {
        var esCliente = _cmbRol.SelectedItem is NombreRol.Cliente;
        _lblNombre.Visible = _txtNombre.Visible = esCliente;
        _lblApellido.Visible = _txtApellido.Visible = esCliente;
        _lblDni.Visible = _txtDni.Visible = esCliente;
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_txtClave.Text != _txtConfirmarClave.Text)
        {
            MessageBox.Show(this, GestorIdioma.Instancia.Traducir("msg.clavesnocoinciden"),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var rol = (NombreRol)_cmbRol.SelectedItem!;
            _gestorAutenticacion.Registrar(
                _txtUsuario.Text.Trim(), _txtClave.Text, rol,
                _txtNombre.Text, _txtApellido.Text, _txtDni.Text);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (AutoVentasException ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    public void ActualizarIdioma()
    {
        var t = GestorIdioma.Instancia;
        Text = t.Traducir("frm.registro");
        _lblUsuario.Text = t.Traducir("lbl.usuario");
        _lblClave.Text = t.Traducir("lbl.clave");
        _lblConfirmarClave.Text = t.Traducir("lbl.confirmarclave");
        _lblRol.Text = t.Traducir("lbl.rol");
        _lblNombre.Text = t.Traducir("lbl.nombre");
        _lblApellido.Text = t.Traducir("lbl.apellido");
        _lblDni.Text = t.Traducir("lbl.dni");
        _btnGuardar.Text = t.Traducir("btn.guardar");
        _btnCancelar.Text = t.Traducir("btn.cancelar");
    }
}
