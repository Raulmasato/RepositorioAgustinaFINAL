namespace AutoVentas.UI.Formularios.PortalCliente
{
    partial class FrmCatalogoVehiculos
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
            this._grilla = new System.Windows.Forms.DataGridView();
            this._panelBotones = new System.Windows.Forms.FlowLayoutPanel();
            this._btnReservar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _grilla
            //
            this._grilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grilla.ReadOnly = true;
            this._grilla.AllowUserToAddRows = false;
            this._grilla.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grilla.MultiSelect = false;
            this._grilla.AutoGenerateColumns = false;
            this._grilla.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grilla.Name = "_grilla";
            this._grilla.TabIndex = 0;
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Vehiculo.IdVehiculo), HeaderText = "Id", Width = 50, Visible = false });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Vehiculo.Marca), HeaderText = "Marca" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Vehiculo.Modelo), HeaderText = "Modelo" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Vehiculo.Color), HeaderText = "Color" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Vehiculo.Anio), HeaderText = "Año" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Vehiculo.Precio), HeaderText = "Precio" });
            //
            // _panelBotones
            //
            this._panelBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelBotones.Height = 42;
            this._panelBotones.Padding = new System.Windows.Forms.Padding(6);
            this._panelBotones.Name = "_panelBotones";
            this._panelBotones.TabIndex = 1;
            this._panelBotones.Controls.Add(this._btnReservar);
            //
            // _btnReservar
            //
            this._btnReservar.AutoSize = true;
            this._btnReservar.Name = "_btnReservar";
            this._btnReservar.TabIndex = 0;
            this._btnReservar.Text = "Reservar";
            this._btnReservar.UseVisualStyleBackColor = true;
            this._btnReservar.Click += new System.EventHandler(this.BtnReservar_Click);
            //
            // FrmCatalogoVehiculos
            //
            this.Text = "Vehículos";
            this.Width = 760;
            this.Height = 480;
            this.Controls.Add(this._grilla);
            this.Controls.Add(this._panelBotones);
            this.Name = "FrmCatalogoVehiculos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView _grilla;
        private System.Windows.Forms.FlowLayoutPanel _panelBotones;
        private System.Windows.Forms.Button _btnReservar;
    }
}
