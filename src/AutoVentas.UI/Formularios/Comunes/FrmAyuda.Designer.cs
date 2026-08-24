namespace AutoVentas.UI.Formularios.Comunes
{
    partial class FrmAyuda
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
            this._listaTemas = new System.Windows.Forms.ListBox();
            this._txtContenido = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // _listaTemas
            //
            this._listaTemas.Dock = System.Windows.Forms.DockStyle.Left;
            this._listaTemas.DisplayMember = "Titulo";
            this._listaTemas.Location = new System.Drawing.Point(0, 0);
            this._listaTemas.Name = "_listaTemas";
            this._listaTemas.Size = new System.Drawing.Size(220, 96);
            this._listaTemas.TabIndex = 0;
            this._listaTemas.SelectedIndexChanged += new System.EventHandler(this.ListaTemas_SelectedIndexChanged);
            //
            // _txtContenido
            //
            this._txtContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtContenido.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 10F);
            this._txtContenido.Location = new System.Drawing.Point(220, 0);
            this._txtContenido.Multiline = true;
            this._txtContenido.Name = "_txtContenido";
            this._txtContenido.ReadOnly = true;
            this._txtContenido.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtContenido.Size = new System.Drawing.Size(480, 458);
            this._txtContenido.TabIndex = 1;
            //
            // FrmAyuda
            //
            this.Controls.Add(this._txtContenido);
            this.Controls.Add(this._listaTemas);
            this.Name = "FrmAyuda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ayuda";
            this.Width = 700;
            this.Height = 480;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox _listaTemas;
        private System.Windows.Forms.TextBox _txtContenido;
    }
}
