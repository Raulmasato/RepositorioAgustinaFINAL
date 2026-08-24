namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmPermisos
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
            this._lblRol = new System.Windows.Forms.Label();
            this._cmbRol = new System.Windows.Forms.ComboBox();
            this._arbol = new System.Windows.Forms.TreeView();
            this._panelSuperior = new System.Windows.Forms.Panel();
            this._btnGuardar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _lblRol
            //
            this._lblRol.Location = new System.Drawing.Point(10, 14);
            this._lblRol.Name = "_lblRol";
            this._lblRol.Size = new System.Drawing.Size(60, 23);
            this._lblRol.TabIndex = 0;
            this._lblRol.Text = "Rol";
            //
            // _cmbRol
            //
            this._cmbRol.Location = new System.Drawing.Point(75, 10);
            this._cmbRol.Name = "_cmbRol";
            this._cmbRol.Size = new System.Drawing.Size(200, 23);
            this._cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRol.DisplayMember = "Nombre";
            this._cmbRol.TabIndex = 1;
            this._cmbRol.SelectedIndexChanged += new System.EventHandler(this.CmbRol_SelectedIndexChanged);
            //
            // _arbol
            //
            this._arbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this._arbol.CheckBoxes = true;
            this._arbol.Name = "_arbol";
            this._arbol.TabIndex = 2;
            this._arbol.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.Arbol_AfterCheck);
            //
            // _btnGuardar
            //
            this._btnGuardar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._btnGuardar.Height = 34;
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.TabIndex = 3;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _panelSuperior
            //
            this._panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelSuperior.Height = 42;
            this._panelSuperior.Name = "_panelSuperior";
            this._panelSuperior.TabIndex = 4;
            this._panelSuperior.Controls.Add(this._lblRol);
            this._panelSuperior.Controls.Add(this._cmbRol);
            //
            // FrmPermisos
            //
            this.Controls.Add(this._arbol);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._panelSuperior);
            this.Text = "Permisos";
            this.Width = 520;
            this.Height = 600;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name = "FrmPermisos";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblRol;
        private System.Windows.Forms.ComboBox _cmbRol;
        private System.Windows.Forms.TreeView _arbol;
        private System.Windows.Forms.Panel _panelSuperior;
        private System.Windows.Forms.Button _btnGuardar;
    }
}
