namespace AutoVentas.UI.Formularios
{
    partial class FrmPrincipal
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
            this._lblBienvenida = new System.Windows.Forms.Label();
            this._btnIrAlMenu = new System.Windows.Forms.Button();
            this._btnCerrarSesion = new System.Windows.Forms.Button();
            this._lblIdioma = new System.Windows.Forms.Label();
            this._selectorIdioma = new AutoVentas.UI.Formularios.Comunes.SelectorIdioma();
            this._traducir = new System.Windows.Forms.Button();
            this._btnAyuda = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _lblBienvenida
            //
            this._lblBienvenida.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 12F);
            this._lblBienvenida.Location = new System.Drawing.Point(40, 40);
            this._lblBienvenida.Name = "_lblBienvenida";
            this._lblBienvenida.Size = new System.Drawing.Size(400, 23);
            this._lblBienvenida.TabIndex = 0;
            //
            // _btnIrAlMenu
            //
            this._btnIrAlMenu.Location = new System.Drawing.Point(40, 90);
            this._btnIrAlMenu.Name = "_btnIrAlMenu";
            this._btnIrAlMenu.Size = new System.Drawing.Size(200, 40);
            this._btnIrAlMenu.TabIndex = 1;
            this._btnIrAlMenu.Text = "Ir a mi menú";
            this._btnIrAlMenu.UseVisualStyleBackColor = true;
            this._btnIrAlMenu.Click += new System.EventHandler(this.BtnIrAlMenu_Click);
            //
            // _btnCerrarSesion
            //
            this._btnCerrarSesion.Location = new System.Drawing.Point(260, 90);
            this._btnCerrarSesion.Name = "_btnCerrarSesion";
            this._btnCerrarSesion.Size = new System.Drawing.Size(150, 40);
            this._btnCerrarSesion.TabIndex = 2;
            this._btnCerrarSesion.Text = "Cerrar sesión";
            this._btnCerrarSesion.UseVisualStyleBackColor = true;
            this._btnCerrarSesion.Click += new System.EventHandler(this.BtnCerrarSesion_Click);
            //
            // _lblIdioma
            //
            this._lblIdioma.Location = new System.Drawing.Point(40, 130);
            this._lblIdioma.Name = "_lblIdioma";
            this._lblIdioma.Size = new System.Drawing.Size(200, 23);
            this._lblIdioma.TabIndex = 3;
            this._lblIdioma.Text = "Idioma";
            //
            // _selectorIdioma
            //
            this._selectorIdioma.Location = new System.Drawing.Point(40, 150);
            this._selectorIdioma.Name = "_selectorIdioma";
            this._selectorIdioma.Size = new System.Drawing.Size(130, 23);
            this._selectorIdioma.TabIndex = 4;
            //
            // _traducir
            //
            this._traducir.Location = new System.Drawing.Point(175, 149);
            this._traducir.Name = "_traducir";
            this._traducir.Size = new System.Drawing.Size(65, 23);
            this._traducir.TabIndex = 5;
            this._traducir.Text = "Traducir";
            this._traducir.UseVisualStyleBackColor = true;
            this._traducir.Click += new System.EventHandler(this.Traducir_Click);
            //
            // _btnAyuda
            //
            this._btnAyuda.Location = new System.Drawing.Point(250, 150);
            this._btnAyuda.Name = "_btnAyuda";
            this._btnAyuda.Size = new System.Drawing.Size(150, 25);
            this._btnAyuda.TabIndex = 6;
            this._btnAyuda.Text = "Ayuda";
            this._btnAyuda.UseVisualStyleBackColor = true;
            this._btnAyuda.Click += new System.EventHandler(this.BtnAyuda_Click);
            //
            // FrmPrincipal
            //
            this.Text = "Sistema de Venta de Autos";
            this.Width = 480;
            this.Height = 260;
            this.Controls.Add(this._lblBienvenida);
            this.Controls.Add(this._btnIrAlMenu);
            this.Controls.Add(this._btnCerrarSesion);
            this.Controls.Add(this._lblIdioma);
            this.Controls.Add(this._selectorIdioma);
            this.Controls.Add(this._traducir);
            this.Controls.Add(this._btnAyuda);
            this.MaximizeBox = false;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblBienvenida;
        private System.Windows.Forms.Button _btnIrAlMenu;
        private System.Windows.Forms.Button _btnCerrarSesion;
        private System.Windows.Forms.Label _lblIdioma;
        private AutoVentas.UI.Formularios.Comunes.SelectorIdioma _selectorIdioma;
        private System.Windows.Forms.Button _traducir;
        private System.Windows.Forms.Button _btnAyuda;
    }
}
