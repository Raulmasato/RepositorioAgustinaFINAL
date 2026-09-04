namespace AutoVentas.UI.Formularios.Comunes
{
    partial class FormMenuRolBase
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
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._menuOpciones = new System.Windows.Forms.ToolStripMenuItem();
            this._menuAyuda = new System.Windows.Forms.ToolStripMenuItem();
            this._menuVolver = new System.Windows.Forms.ToolStripMenuItem();
            this._menuTraducir = new System.Windows.Forms.ToolStripMenuItem();
            this._selectorIdioma = new AutoVentas.UI.Formularios.Comunes.SelectorIdioma();
            this._hostSelectorIdioma = new System.Windows.Forms.ToolStripControlHost(this._selectorIdioma);
            this._menuStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // _menuOpciones
            //
            this._menuOpciones.Name = "_menuOpciones";
            //
            // _menuAyuda
            //
            this._menuAyuda.Name = "_menuAyuda";
            this._menuAyuda.Text = "Ayuda";
            this._menuAyuda.Click += new System.EventHandler(this.MenuAyuda_Click);
            //
            // _menuVolver
            //
            this._menuVolver.Name = "_menuVolver";
            this._menuVolver.Text = "Volver";
            this._menuVolver.Click += new System.EventHandler(this.MenuVolver_Click);
            //
            // _selectorIdioma
            //
            this._selectorIdioma.Name = "_selectorIdioma";
            //
            // _hostSelectorIdioma
            //
            this._hostSelectorIdioma.Name = "_hostSelectorIdioma";
            //
            // _menuTraducir
            //
            this._menuTraducir.Name = "_menuTraducir";
            this._menuTraducir.Text = "Traducir";
            this._menuTraducir.Click += new System.EventHandler(this.Traducir_Click);
            //
            // _menuStrip
            //
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuOpciones,
            this._menuAyuda,
            this._menuVolver,
            this._hostSelectorIdioma,
            this._menuTraducir});
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.TabIndex = 0;
            //
            // FormMenuRolBase
            //
            this.Controls.Add(this._menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this._menuStrip;
            this.Name = "FormMenuRolBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Width = 1000;
            this.Height = 650;
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _menuOpciones;
        private System.Windows.Forms.ToolStripMenuItem _menuAyuda;
        private System.Windows.Forms.ToolStripMenuItem _menuVolver;
        private System.Windows.Forms.ToolStripMenuItem _menuTraducir;
        private AutoVentas.UI.Formularios.Comunes.SelectorIdioma _selectorIdioma;
        private System.Windows.Forms.ToolStripControlHost _hostSelectorIdioma;
    }
}
