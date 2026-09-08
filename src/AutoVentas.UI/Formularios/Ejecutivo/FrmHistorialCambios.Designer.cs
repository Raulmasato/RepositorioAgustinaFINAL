namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmHistorialCambios
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
            this._lblTabla = new System.Windows.Forms.Label();
            this._cmbTabla = new System.Windows.Forms.ComboBox();
            this._lblId = new System.Windows.Forms.Label();
            this._numId = new System.Windows.Forms.NumericUpDown();
            this._btnBuscar = new System.Windows.Forms.Button();
            this._grilla = new System.Windows.Forms.DataGridView();
            this._panelFiltros = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._numId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).BeginInit();
            this.SuspendLayout();
            //
            // _lblTabla
            //
            this._lblTabla.Location = new System.Drawing.Point(10, 14);
            this._lblTabla.Name = "_lblTabla";
            this._lblTabla.Size = new System.Drawing.Size(45, 23);
            this._lblTabla.TabIndex = 0;
            this._lblTabla.Text = "Tabla";
            //
            // _cmbTabla
            //
            this._cmbTabla.Location = new System.Drawing.Point(55, 10);
            this._cmbTabla.Name = "_cmbTabla";
            this._cmbTabla.Size = new System.Drawing.Size(160, 23);
            this._cmbTabla.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTabla.TabIndex = 1;
            //
            // _lblId
            //
            this._lblId.Location = new System.Drawing.Point(225, 14);
            this._lblId.Name = "_lblId";
            this._lblId.Size = new System.Drawing.Size(25, 23);
            this._lblId.TabIndex = 2;
            this._lblId.Text = "Id";
            //
            // _numId
            //
            this._numId.Location = new System.Drawing.Point(255, 10);
            this._numId.Name = "_numId";
            this._numId.Size = new System.Drawing.Size(80, 23);
            this._numId.Minimum = 1;
            this._numId.Maximum = int.MaxValue;
            this._numId.TabIndex = 3;
            //
            // _btnBuscar
            //
            this._btnBuscar.Location = new System.Drawing.Point(345, 8);
            this._btnBuscar.Name = "_btnBuscar";
            this._btnBuscar.Size = new System.Drawing.Size(90, 23);
            this._btnBuscar.TabIndex = 4;
            this._btnBuscar.Text = "Buscar";
            this._btnBuscar.UseVisualStyleBackColor = true;
            this._btnBuscar.Click += new System.EventHandler(this.BtnBuscar_Click);
            //
            // _grilla
            //
            this._grilla.Location = new System.Drawing.Point(0, 42);
            this._grilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grilla.ReadOnly = true;
            this._grilla.AllowUserToAddRows = false;
            this._grilla.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grilla.AutoGenerateColumns = false;
            this._grilla.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grilla.Name = "_grilla";
            this._grilla.TabIndex = 5;
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "FechaHora", HeaderText = "Fecha/Hora", Width = 140 });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "NombreUsuario", HeaderText = "Usuario" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "TipoOperacion", HeaderText = "Operación" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "Campo", HeaderText = "Campo" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "ValorAnterior", HeaderText = "Valor anterior" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "ValorNuevo", HeaderText = "Valor nuevo" });
            //
            // _panelFiltros
            //
            this._panelFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelFiltros.Height = 42;
            this._panelFiltros.Name = "_panelFiltros";
            this._panelFiltros.TabIndex = 6;
            this._panelFiltros.Controls.Add(this._lblTabla);
            this._panelFiltros.Controls.Add(this._cmbTabla);
            this._panelFiltros.Controls.Add(this._lblId);
            this._panelFiltros.Controls.Add(this._numId);
            this._panelFiltros.Controls.Add(this._btnBuscar);
            //
            // FrmHistorialCambios
            //
            this.Controls.Add(this._grilla);
            this.Controls.Add(this._panelFiltros);
            this.Text = "Historial de cambios";
            this.Width = 900;
            this.Height = 520;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name = "FrmHistorialCambios";
            ((System.ComponentModel.ISupportInitialize)(this._numId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblTabla;
        private System.Windows.Forms.ComboBox _cmbTabla;
        private System.Windows.Forms.Label _lblId;
        private System.Windows.Forms.NumericUpDown _numId;
        private System.Windows.Forms.Button _btnBuscar;
        private System.Windows.Forms.DataGridView _grilla;
        private System.Windows.Forms.Panel _panelFiltros;
    }
}
