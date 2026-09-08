namespace AutoVentas.UI.Formularios
{
    partial class FrmRegistro
    {
        /// <summary>Variable de diseñador requerida.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Limpia los recursos que se estén utilizando.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No modificar el contenido
        /// de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this._lblUsuario = new System.Windows.Forms.Label();
            this._txtUsuario = new System.Windows.Forms.TextBox();
            this._lblClave = new System.Windows.Forms.Label();
            this._txtClave = new System.Windows.Forms.TextBox();
            this._lblConfirmarClave = new System.Windows.Forms.Label();
            this._txtConfirmarClave = new System.Windows.Forms.TextBox();
            this._lblRol = new System.Windows.Forms.Label();
            this._cmbRol = new System.Windows.Forms.ComboBox();
            this._lblNombre = new System.Windows.Forms.Label();
            this._txtNombre = new System.Windows.Forms.TextBox();
            this._lblApellido = new System.Windows.Forms.Label();
            this._txtApellido = new System.Windows.Forms.TextBox();
            this._lblDni = new System.Windows.Forms.Label();
            this._txtDni = new System.Windows.Forms.TextBox();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _lblUsuario
            //
            this._lblUsuario.Location = new System.Drawing.Point(20, 20);
            this._lblUsuario.Name = "_lblUsuario";
            this._lblUsuario.Size = new System.Drawing.Size(120, 23);
            this._lblUsuario.TabIndex = 0;
            this._lblUsuario.Text = "Usuario";
            //
            // _txtUsuario
            //
            this._txtUsuario.Location = new System.Drawing.Point(150, 17);
            this._txtUsuario.Name = "_txtUsuario";
            this._txtUsuario.Size = new System.Drawing.Size(220, 23);
            this._txtUsuario.TabIndex = 1;
            //
            // _lblClave
            //
            this._lblClave.Location = new System.Drawing.Point(20, 55);
            this._lblClave.Name = "_lblClave";
            this._lblClave.Size = new System.Drawing.Size(120, 23);
            this._lblClave.TabIndex = 2;
            this._lblClave.Text = "Contraseña";
            //
            // _txtClave
            //
            this._txtClave.Location = new System.Drawing.Point(150, 52);
            this._txtClave.Name = "_txtClave";
            this._txtClave.Size = new System.Drawing.Size(220, 23);
            this._txtClave.TabIndex = 3;
            this._txtClave.UseSystemPasswordChar = true;
            //
            // _lblConfirmarClave
            //
            this._lblConfirmarClave.Location = new System.Drawing.Point(20, 90);
            this._lblConfirmarClave.Name = "_lblConfirmarClave";
            this._lblConfirmarClave.Size = new System.Drawing.Size(120, 23);
            this._lblConfirmarClave.TabIndex = 4;
            this._lblConfirmarClave.Text = "Confirmar contraseña";
            //
            // _txtConfirmarClave
            //
            this._txtConfirmarClave.Location = new System.Drawing.Point(150, 87);
            this._txtConfirmarClave.Name = "_txtConfirmarClave";
            this._txtConfirmarClave.Size = new System.Drawing.Size(220, 23);
            this._txtConfirmarClave.TabIndex = 5;
            this._txtConfirmarClave.UseSystemPasswordChar = true;
            //
            // _lblRol
            //
            this._lblRol.Location = new System.Drawing.Point(20, 125);
            this._lblRol.Name = "_lblRol";
            this._lblRol.Size = new System.Drawing.Size(120, 23);
            this._lblRol.TabIndex = 6;
            this._lblRol.Text = "Rol";
            //
            // _cmbRol
            //
            this._cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRol.Location = new System.Drawing.Point(150, 122);
            this._cmbRol.Name = "_cmbRol";
            this._cmbRol.Size = new System.Drawing.Size(220, 23);
            this._cmbRol.TabIndex = 7;
            this._cmbRol.SelectedIndexChanged += new System.EventHandler(this.CmbRol_SelectedIndexChanged);
            //
            // _lblNombre
            //
            this._lblNombre.Location = new System.Drawing.Point(20, 160);
            this._lblNombre.Name = "_lblNombre";
            this._lblNombre.Size = new System.Drawing.Size(120, 23);
            this._lblNombre.TabIndex = 8;
            this._lblNombre.Text = "Nombre";
            //
            // _txtNombre
            //
            this._txtNombre.Location = new System.Drawing.Point(150, 157);
            this._txtNombre.Name = "_txtNombre";
            this._txtNombre.Size = new System.Drawing.Size(220, 23);
            this._txtNombre.TabIndex = 9;
            //
            // _lblApellido
            //
            this._lblApellido.Location = new System.Drawing.Point(20, 195);
            this._lblApellido.Name = "_lblApellido";
            this._lblApellido.Size = new System.Drawing.Size(120, 23);
            this._lblApellido.TabIndex = 10;
            this._lblApellido.Text = "Apellido";
            //
            // _txtApellido
            //
            this._txtApellido.Location = new System.Drawing.Point(150, 192);
            this._txtApellido.Name = "_txtApellido";
            this._txtApellido.Size = new System.Drawing.Size(220, 23);
            this._txtApellido.TabIndex = 11;
            //
            // _lblDni
            //
            this._lblDni.Location = new System.Drawing.Point(20, 230);
            this._lblDni.Name = "_lblDni";
            this._lblDni.Size = new System.Drawing.Size(120, 23);
            this._lblDni.TabIndex = 12;
            this._lblDni.Text = "DNI";
            //
            // _txtDni
            //
            this._txtDni.Location = new System.Drawing.Point(150, 227);
            this._txtDni.Name = "_txtDni";
            this._txtDni.Size = new System.Drawing.Size(220, 23);
            this._txtDni.TabIndex = 13;
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(150, 270);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(100, 23);
            this._btnGuardar.TabIndex = 14;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _btnCancelar
            //
            this._btnCancelar.Location = new System.Drawing.Point(270, 270);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(100, 23);
            this._btnCancelar.TabIndex = 15;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FrmRegistro
            //
            this.Text = "Registro de usuario";
            this.Width = 420;
            this.Height = 360;
            this.Controls.Add(this._lblUsuario);
            this.Controls.Add(this._txtUsuario);
            this.Controls.Add(this._lblClave);
            this.Controls.Add(this._txtClave);
            this.Controls.Add(this._lblConfirmarClave);
            this.Controls.Add(this._txtConfirmarClave);
            this.Controls.Add(this._lblRol);
            this.Controls.Add(this._cmbRol);
            this.Controls.Add(this._lblNombre);
            this.Controls.Add(this._txtNombre);
            this.Controls.Add(this._lblApellido);
            this.Controls.Add(this._txtApellido);
            this.Controls.Add(this._lblDni);
            this.Controls.Add(this._txtDni);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRegistro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblUsuario;
        private System.Windows.Forms.TextBox _txtUsuario;
        private System.Windows.Forms.Label _lblClave;
        private System.Windows.Forms.TextBox _txtClave;
        private System.Windows.Forms.Label _lblConfirmarClave;
        private System.Windows.Forms.TextBox _txtConfirmarClave;
        private System.Windows.Forms.Label _lblRol;
        private System.Windows.Forms.ComboBox _cmbRol;
        private System.Windows.Forms.Label _lblNombre;
        private System.Windows.Forms.TextBox _txtNombre;
        private System.Windows.Forms.Label _lblApellido;
        private System.Windows.Forms.TextBox _txtApellido;
        private System.Windows.Forms.Label _lblDni;
        private System.Windows.Forms.TextBox _txtDni;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}
