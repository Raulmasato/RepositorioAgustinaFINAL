namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmNuevoIdioma
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
            this._lblCodigo = new System.Windows.Forms.Label();
            this._txtCodigo = new System.Windows.Forms.TextBox();
            this._lblNombre = new System.Windows.Forms.Label();
            this._txtNombre = new System.Windows.Forms.TextBox();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _lblCodigo
            //
            this._lblCodigo.Location = new System.Drawing.Point(20, 20);
            this._lblCodigo.Name = "_lblCodigo";
            this._lblCodigo.Size = new System.Drawing.Size(100, 23);
            this._lblCodigo.TabIndex = 0;
            this._lblCodigo.Text = "Código (ej: it)";
            //
            // _txtCodigo
            //
            this._txtCodigo.Location = new System.Drawing.Point(130, 17);
            this._txtCodigo.Name = "_txtCodigo";
            this._txtCodigo.Size = new System.Drawing.Size(100, 23);
            this._txtCodigo.MaxLength = 10;
            this._txtCodigo.TabIndex = 1;
            //
            // _lblNombre
            //
            this._lblNombre.Location = new System.Drawing.Point(20, 55);
            this._lblNombre.Name = "_lblNombre";
            this._lblNombre.Size = new System.Drawing.Size(100, 23);
            this._lblNombre.TabIndex = 2;
            this._lblNombre.Text = "Nombre";
            //
            // _txtNombre
            //
            this._txtNombre.Location = new System.Drawing.Point(130, 52);
            this._txtNombre.Name = "_txtNombre";
            this._txtNombre.Size = new System.Drawing.Size(200, 23);
            this._txtNombre.TabIndex = 3;
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(130, 90);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(90, 23);
            this._btnGuardar.TabIndex = 4;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _btnCancelar
            //
            this._btnCancelar.Location = new System.Drawing.Point(230, 90);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(90, 23);
            this._btnCancelar.TabIndex = 5;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FrmNuevoIdioma
            //
            this.Controls.Add(this._lblCodigo);
            this.Controls.Add(this._txtCodigo);
            this.Controls.Add(this._lblNombre);
            this.Controls.Add(this._txtNombre);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.Text = "Nuevo idioma";
            this.Width = 380;
            this.Height = 180;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNuevoIdioma";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblCodigo;
        private System.Windows.Forms.TextBox _txtCodigo;
        private System.Windows.Forms.Label _lblNombre;
        private System.Windows.Forms.TextBox _txtNombre;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}
