using AutoVentas.BLL;
using AutoVentas.Domain.Entidades;
using AutoVentas.Services.Idioma;

namespace AutoVentas.UI.Formularios.Vendedor;

internal partial class FrmClienteEditar : Form, IObservadorIdioma
{
    private readonly GestorClientes _gestor = new();
    private readonly Cliente? _original;

    public FrmClienteEditar(Cliente? cliente)
    {
        _original = cliente;

        InitializeComponent();

        if (cliente is not null)
        {
            _txtNombre.Text = cliente.Nombre;
            _txtApellido.Text = cliente.Apellido;
            _txtDni.Text = cliente.DniPlano;
        }

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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
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
