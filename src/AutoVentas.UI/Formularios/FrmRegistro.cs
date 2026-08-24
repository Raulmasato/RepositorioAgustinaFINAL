using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Domain.Excepciones;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios;

/// <summary>Formulario de registro de un nuevo usuario. Si el rol elegido es Cliente,
/// solicita también nombre, apellido y DNI para crear el registro de Cliente asociado.</summary>
public partial class FrmRegistro : Form, IObservadorIdioma
{
    private readonly GestorAutenticacion _gestorAutenticacion = new();

    public FrmRegistro()
    {
        InitializeComponent();

        _cmbRol.Items.AddRange(new object[] { NombreRol.Cliente, NombreRol.Vendedor, NombreRol.Tecnico, NombreRol.Ejecutivo });
        _cmbRol.SelectedIndex = 0;

        GestorIdioma.Instancia.Suscribir(this);
        FormClosed += (_, _) => GestorIdioma.Instancia.Desuscribir(this);
        ActualizarIdioma();
        ActualizarVisibilidadCampoCliente();
    }

    private void CmbRol_SelectedIndexChanged(object? sender, EventArgs e)
    {
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
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
