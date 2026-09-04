namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmBitacora
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
            this._lblActividad = new System.Windows.Forms.Label();
            this._txtActividad = new System.Windows.Forms.TextBox();
            this._lblDesde = new System.Windows.Forms.Label();
            this._dtpDesde = new System.Windows.Forms.DateTimePicker();
            this._lblHasta = new System.Windows.Forms.Label();
            this._dtpHasta = new System.Windows.Forms.DateTimePicker();
            this._btnBuscar = new System.Windows.Forms.Button();
            this._grilla = new System.Windows.Forms.DataGridView();
            this._panelFiltros = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).BeginInit();
            this.SuspendLayout();
            //
            // _lblActividad
            //
            this._lblActividad.Location = new System.Drawing.Point(10, 14);
            this._lblActividad.Name = "_lblActividad";
            this._lblActividad.Size = new System.Drawing.Size(70, 23);
            this._lblActividad.TabIndex = 0;
            this._lblActividad.Text = "Actividad";
            //
            // _txtActividad
            //
            this._txtActividad.Location = new System.Drawing.Point(85, 10);
            this._txtActividad.Name = "_txtActividad";
            this._txtActividad.Size = new System.Drawing.Size(180, 23);
            this._txtActividad.TabIndex = 1;
            //
            // _lblDesde
            //
            this._lblDesde.Location = new System.Drawing.Point(275, 14);
            this._lblDesde.Name = "_lblDesde";
            this._lblDesde.Size = new System.Drawing.Size(45, 23);
            this._lblDesde.TabIndex = 2;
            this._lblDesde.Text = "Desde";
            //
            // _dtpDesde
            //
            this._dtpDesde.Location = new System.Drawing.Point(320, 10);
            this._dtpDesde.Name = "_dtpDesde";
            this._dtpDesde.Size = new System.Drawing.Size(130, 23);
            this._dtpDesde.TabIndex = 3;
            this._dtpDesde.Value = System.DateTime.Now.AddMonths(-1);
            //
            // _lblHasta
            //
            this._lblHasta.Location = new System.Drawing.Point(460, 14);
            this._lblHasta.Name = "_lblHasta";
            this._lblHasta.Size = new System.Drawing.Size(40, 23);
            this._lblHasta.TabIndex = 4;
            this._lblHasta.Text = "Hasta";
            //
            // _dtpHasta
            //
            this._dtpHasta.Location = new System.Drawing.Point(500, 10);
            this._dtpHasta.Name = "_dtpHasta";
            this._dtpHasta.Size = new System.Drawing.Size(130, 23);
            this._dtpHasta.TabIndex = 5;
            //
            // _btnBuscar
            //
            this._btnBuscar.Location = new System.Drawing.Point(640, 8);
            this._btnBuscar.Name = "_btnBuscar";
            this._btnBuscar.Size = new System.Drawing.Size(90, 23);
            this._btnBuscar.TabIndex = 6;
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
            this._grilla.TabIndex = 7;
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "FechaHora", HeaderText = "Fecha/Hora", Width = 140 });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "NombreUsuario", HeaderText = "Usuario" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "Actividad", HeaderText = "Actividad" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "Informacion", HeaderText = "Información" });
            //
            // _panelFiltros
            //
            this._panelFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelFiltros.Height = 42;
            this._panelFiltros.Name = "_panelFiltros";
            this._panelFiltros.TabIndex = 8;
            this._panelFiltros.Controls.Add(this._lblActividad);
            this._panelFiltros.Controls.Add(this._txtActividad);
            this._panelFiltros.Controls.Add(this._lblDesde);
            this._panelFiltros.Controls.Add(this._dtpDesde);
            this._panelFiltros.Controls.Add(this._lblHasta);
            this._panelFiltros.Controls.Add(this._dtpHasta);
            this._panelFiltros.Controls.Add(this._btnBuscar);
            //
            // FrmBitacora
            //
            this.Controls.Add(this._grilla);
            this.Controls.Add(this._panelFiltros);
            this.Text = "Bitácora";
            this.Width = 900;
            this.Height = 520;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name = "FrmBitacora";
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblActividad;
        private System.Windows.Forms.TextBox _txtActividad;
        private System.Windows.Forms.Label _lblDesde;
        private System.Windows.Forms.DateTimePicker _dtpDesde;
        private System.Windows.Forms.Label _lblHasta;
        private System.Windows.Forms.DateTimePicker _dtpHasta;
        private System.Windows.Forms.Button _btnBuscar;
        private System.Windows.Forms.DataGridView _grilla;
        private System.Windows.Forms.Panel _panelFiltros;
    }
}
