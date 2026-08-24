namespace AutoVentas.UI.Formularios.PortalCliente
{
    partial class FrmMisReservas
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
            this._btnNuevaReserva = new System.Windows.Forms.Button();
            this._btnRefrescar = new System.Windows.Forms.Button();
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
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Reserva.IdReserva), HeaderText = "Id", Width = 50 });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Reserva.VehiculoDescripcion), HeaderText = "Vehículo" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Reserva.FechaReserva), HeaderText = "Fecha reserva" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Reserva.FechaVencimiento), HeaderText = "Vence" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Reserva.Estado), HeaderText = "Estado" });
            //
            // _panelBotones
            //
            this._panelBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelBotones.Height = 42;
            this._panelBotones.Padding = new System.Windows.Forms.Padding(6);
            this._panelBotones.Name = "_panelBotones";
            this._panelBotones.TabIndex = 1;
            this._panelBotones.Controls.Add(this._btnNuevaReserva);
            this._panelBotones.Controls.Add(this._btnRefrescar);
            //
            // _btnNuevaReserva
            //
            this._btnNuevaReserva.AutoSize = true;
            this._btnNuevaReserva.Name = "_btnNuevaReserva";
            this._btnNuevaReserva.TabIndex = 0;
            this._btnNuevaReserva.Text = "Nueva reserva";
            this._btnNuevaReserva.UseVisualStyleBackColor = true;
            this._btnNuevaReserva.Click += new System.EventHandler(this.BtnNuevaReserva_Click);
            //
            // _btnRefrescar
            //
            this._btnRefrescar.AutoSize = true;
            this._btnRefrescar.Name = "_btnRefrescar";
            this._btnRefrescar.TabIndex = 1;
            this._btnRefrescar.Text = "Refrescar";
            this._btnRefrescar.UseVisualStyleBackColor = true;
            this._btnRefrescar.Click += new System.EventHandler(this.BtnRefrescar_Click);
            //
            // FrmMisReservas
            //
            this.Text = "Reservas";
            this.Width = 760;
            this.Height = 480;
            this.Controls.Add(this._grilla);
            this.Controls.Add(this._panelBotones);
            this.Name = "FrmMisReservas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView _grilla;
        private System.Windows.Forms.FlowLayoutPanel _panelBotones;
        private System.Windows.Forms.Button _btnNuevaReserva;
        private System.Windows.Forms.Button _btnRefrescar;
    }
}
