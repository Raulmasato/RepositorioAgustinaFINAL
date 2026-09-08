namespace AutoVentas.UI.Formularios
{
    partial class FrmLogin
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
            this._btnIngresar = new System.Windows.Forms.Button();
            this._btnRegistrarse = new System.Windows.Forms.Button();
            this._selectorIdioma = new AutoVentas.UI.Formularios.Comunes.SelectorIdioma();
            this._traducir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _lblUsuario
            //
            this._lblUsuario.Location = new System.Drawing.Point(30, 43);
            this._lblUsuario.Name = "_lblUsuario";
            this._lblUsuario.Size = new System.Drawing.Size(100, 23);
            this._lblUsuario.TabIndex = 0;
            this._lblUsuario.Text = "Usuario";
            //
            // _txtUsuario
            //
            this._txtUsuario.Location = new System.Drawing.Point(140, 40);
            this._txtUsuario.Name = "_txtUsuario";
            this._txtUsuario.Size = new System.Drawing.Size(200, 23);
            this._txtUsuario.TabIndex = 1;
            //
            // _lblClave
            //
            this._lblClave.Location = new System.Drawing.Point(30, 78);
            this._lblClave.Name = "_lblClave";
            this._lblClave.Size = new System.Drawing.Size(100, 23);
            this._lblClave.TabIndex = 2;
            this._lblClave.Text = "Contraseña";
            //
            // _txtClave
            //
            this._txtClave.Location = new System.Drawing.Point(140, 75);
            this._txtClave.Name = "_txtClave";
            this._txtClave.Size = new System.Drawing.Size(200, 23);
            this._txtClave.TabIndex = 3;
            this._txtClave.UseSystemPasswordChar = true;
            //
            // _btnIngresar
            //
            this._btnIngresar.Location = new System.Drawing.Point(140, 115);
            this._btnIngresar.Name = "_btnIngresar";
            this._btnIngresar.Size = new System.Drawing.Size(95, 23);
            this._btnIngresar.TabIndex = 4;
            this._btnIngresar.Text = "Ingresar";
            this._btnIngresar.UseVisualStyleBackColor = true;
            this._btnIngresar.Click += new System.EventHandler(this.BtnIngresar_Click);
            //
            // _btnRegistrarse
            //
            this._btnRegistrarse.Location = new System.Drawing.Point(245, 115);
            this._btnRegistrarse.Name = "_btnRegistrarse";
            this._btnRegistrarse.Size = new System.Drawing.Size(95, 23);
            this._btnRegistrarse.TabIndex = 5;
            this._btnRegistrarse.Text = "Registrarse";
            this._btnRegistrarse.UseVisualStyleBackColor = true;
            this._btnRegistrarse.Click += new System.EventHandler(this.BtnRegistrarse_Click);
            //
            // _selectorIdioma
            //
            this._selectorIdioma.Location = new System.Drawing.Point(140, 155);
            this._selectorIdioma.Name = "_selectorIdioma";
            this._selectorIdioma.Size = new System.Drawing.Size(200, 23);
            this._selectorIdioma.TabIndex = 6;
            //
            // _traducir
            //
            this._traducir.Location = new System.Drawing.Point(345, 153);
            this._traducir.Name = "_traducir";
            this._traducir.Size = new System.Drawing.Size(65, 23);
            this._traducir.TabIndex = 7;
            this._traducir.Text = "Traducir";
            this._traducir.UseVisualStyleBackColor = true;
            this._traducir.Click += new System.EventHandler(this.Traducir_Click);
            //
            // FrmLogin
            //
            this.AcceptButton = this._btnIngresar;
            this.Text = "Iniciar sesión";
            this.Width = 420;
            this.Height = 260;
            this.Controls.Add(this._lblUsuario);
            this.Controls.Add(this._txtUsuario);
            this.Controls.Add(this._lblClave);
            this.Controls.Add(this._txtClave);
            this.Controls.Add(this._btnIngresar);
            this.Controls.Add(this._btnRegistrarse);
            this.Controls.Add(this._selectorIdioma);
            this.Controls.Add(this._traducir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblUsuario;
        private System.Windows.Forms.TextBox _txtUsuario;
        private System.Windows.Forms.Label _lblClave;
        private System.Windows.Forms.TextBox _txtClave;
        private System.Windows.Forms.Button _btnIngresar;
        private System.Windows.Forms.Button _btnRegistrarse;
        private AutoVentas.UI.Formularios.Comunes.SelectorIdioma _selectorIdioma;
        private System.Windows.Forms.Button _traducir;
    }
}
