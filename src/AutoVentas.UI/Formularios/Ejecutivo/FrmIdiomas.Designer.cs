namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmIdiomas
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
            this._cmbIdioma = new System.Windows.Forms.ComboBox();
            this._btnNuevoIdioma = new System.Windows.Forms.Button();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._grilla = new System.Windows.Forms.DataGridView();
            this._panelSuperior = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).BeginInit();
            this.SuspendLayout();
            //
            // _cmbIdioma
            //
            this._cmbIdioma.Location = new System.Drawing.Point(10, 10);
            this._cmbIdioma.Name = "_cmbIdioma";
            this._cmbIdioma.Size = new System.Drawing.Size(220, 23);
            this._cmbIdioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbIdioma.DisplayMember = "Nombre";
            this._cmbIdioma.TabIndex = 0;
            this._cmbIdioma.SelectedIndexChanged += new System.EventHandler(this.CmbIdioma_SelectedIndexChanged);
            //
            // _btnNuevoIdioma
            //
            this._btnNuevoIdioma.Location = new System.Drawing.Point(240, 8);
            this._btnNuevoIdioma.Name = "_btnNuevoIdioma";
            this._btnNuevoIdioma.Size = new System.Drawing.Size(130, 23);
            this._btnNuevoIdioma.TabIndex = 1;
            this._btnNuevoIdioma.Text = "Nuevo idioma";
            this._btnNuevoIdioma.UseVisualStyleBackColor = true;
            this._btnNuevoIdioma.Click += new System.EventHandler(this.BtnNuevoIdioma_Click);
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(380, 8);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(130, 23);
            this._btnGuardar.TabIndex = 2;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _grilla
            //
            this._grilla.Location = new System.Drawing.Point(0, 44);
            this._grilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grilla.AllowUserToAddRows = false;
            this._grilla.AllowUserToDeleteRows = false;
            this._grilla.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grilla.AutoGenerateColumns = false;
            this._grilla.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grilla.Name = "_grilla";
            this._grilla.TabIndex = 3;
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Clave", HeaderText = "Clave", ReadOnly = true, FillWeight = 30 });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Valor", HeaderText = "Texto en este idioma", FillWeight = 70 });
            //
            // _panelSuperior
            //
            this._panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelSuperior.Height = 44;
            this._panelSuperior.Name = "_panelSuperior";
            this._panelSuperior.TabIndex = 4;
            this._panelSuperior.Controls.Add(this._cmbIdioma);
            this._panelSuperior.Controls.Add(this._btnNuevoIdioma);
            this._panelSuperior.Controls.Add(this._btnGuardar);
            //
            // FrmIdiomas
            //
            this.Controls.Add(this._grilla);
            this.Controls.Add(this._panelSuperior);
            this.Text = "Idiomas";
            this.Width = 700;
            this.Height = 500;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name = "FrmIdiomas";
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox _cmbIdioma;
        private System.Windows.Forms.Button _btnNuevoIdioma;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.DataGridView _grilla;
        private System.Windows.Forms.Panel _panelSuperior;
    }
}
