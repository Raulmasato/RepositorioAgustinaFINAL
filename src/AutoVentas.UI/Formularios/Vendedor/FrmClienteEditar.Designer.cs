namespace AutoVentas.UI.Formularios.Vendedor
{
    partial class FrmClienteEditar
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
            // _lblNombre
            //
            this._lblNombre.Location = new System.Drawing.Point(20, 20);
            this._lblNombre.Name = "_lblNombre";
            this._lblNombre.Size = new System.Drawing.Size(100, 23);
            this._lblNombre.TabIndex = 0;
            this._lblNombre.Text = "Nombre";
            //
            // _txtNombre
            //
            this._txtNombre.Location = new System.Drawing.Point(130, 17);
            this._txtNombre.Name = "_txtNombre";
            this._txtNombre.Size = new System.Drawing.Size(200, 23);
            this._txtNombre.TabIndex = 1;
            //
            // _lblApellido
            //
            this._lblApellido.Location = new System.Drawing.Point(20, 55);
            this._lblApellido.Name = "_lblApellido";
            this._lblApellido.Size = new System.Drawing.Size(100, 23);
            this._lblApellido.TabIndex = 2;
            this._lblApellido.Text = "Apellido";
            //
            // _txtApellido
            //
            this._txtApellido.Location = new System.Drawing.Point(130, 52);
            this._txtApellido.Name = "_txtApellido";
            this._txtApellido.Size = new System.Drawing.Size(200, 23);
            this._txtApellido.TabIndex = 3;
            //
            // _lblDni
            //
            this._lblDni.Location = new System.Drawing.Point(20, 90);
            this._lblDni.Name = "_lblDni";
            this._lblDni.Size = new System.Drawing.Size(100, 23);
            this._lblDni.TabIndex = 4;
            this._lblDni.Text = "DNI";
            //
            // _txtDni
            //
            this._txtDni.Location = new System.Drawing.Point(130, 87);
            this._txtDni.Name = "_txtDni";
            this._txtDni.Size = new System.Drawing.Size(200, 23);
            this._txtDni.TabIndex = 5;
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(130, 130);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(90, 23);
            this._btnGuardar.TabIndex = 6;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _btnCancelar
            //
            this._btnCancelar.Location = new System.Drawing.Point(230, 130);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(90, 23);
            this._btnCancelar.TabIndex = 7;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FrmClienteEditar
            //
            this.Text = "Clientes";
            this.Width = 380;
            this.Height = 220;
            this.Controls.Add(this._lblNombre);
            this.Controls.Add(this._txtNombre);
            this.Controls.Add(this._lblApellido);
            this.Controls.Add(this._txtApellido);
            this.Controls.Add(this._lblDni);
            this.Controls.Add(this._txtDni);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmClienteEditar";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

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
