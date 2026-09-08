namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmEntregas
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
            this._btnNuevo = new System.Windows.Forms.Button();
            this._btnEditar = new System.Windows.Forms.Button();
            this._btnEliminar = new System.Windows.Forms.Button();
            this._btnRefrescar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).BeginInit();
            this.SuspendLayout();
            //
            // _grilla
            //
            this._grilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grilla.ReadOnly = true;
            this._grilla.AllowUserToAddRows = false;
            this._grilla.AllowUserToDeleteRows = false;
            this._grilla.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grilla.MultiSelect = false;
            this._grilla.AutoGenerateColumns = false;
            this._grilla.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grilla.Name = "_grilla";
            this._grilla.TabIndex = 0;
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Entrega.IdEntrega), HeaderText = "Id", Width = 50, Visible = false });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Entrega.ContratoDescripcion), HeaderText = "Contrato" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Entrega.FechaEntrega), HeaderText = "Fecha" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Entrega.LugarEntrega), HeaderText = "Lugar" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = nameof(AutoVentas.Domain.Entidades.Entrega.Estado), HeaderText = "Estado" });
            //
            // _panelBotones
            //
            this._panelBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelBotones.Height = 42;
            this._panelBotones.Padding = new System.Windows.Forms.Padding(6);
            this._panelBotones.Name = "_panelBotones";
            this._panelBotones.TabIndex = 1;
            //
            // _btnNuevo
            //
            this._btnNuevo.AutoSize = true;
            this._btnNuevo.Name = "_btnNuevo";
            this._btnNuevo.TabIndex = 0;
            this._btnNuevo.Text = "Nuevo";
            this._btnNuevo.UseVisualStyleBackColor = true;
            this._btnNuevo.Click += new System.EventHandler(this.BtnNuevo_Click);
            //
            // _btnEditar
            //
            this._btnEditar.AutoSize = true;
            this._btnEditar.Name = "_btnEditar";
            this._btnEditar.TabIndex = 1;
            this._btnEditar.Text = "Editar";
            this._btnEditar.UseVisualStyleBackColor = true;
            this._btnEditar.Click += new System.EventHandler(this.BtnEditar_Click);
            //
            // _btnEliminar
            //
            this._btnEliminar.AutoSize = true;
            this._btnEliminar.Name = "_btnEliminar";
            this._btnEliminar.TabIndex = 2;
            this._btnEliminar.Text = "Eliminar";
            this._btnEliminar.UseVisualStyleBackColor = true;
            this._btnEliminar.Click += new System.EventHandler(this.BtnEliminar_Click);
            //
            // _btnRefrescar
            //
            this._btnRefrescar.AutoSize = true;
            this._btnRefrescar.Name = "_btnRefrescar";
            this._btnRefrescar.TabIndex = 3;
            this._btnRefrescar.Text = "Refrescar";
            this._btnRefrescar.UseVisualStyleBackColor = true;
            this._btnRefrescar.Click += new System.EventHandler(this.BtnRefrescar_Click);
            //
            // _panelBotones (contenido)
            //
            this._panelBotones.Controls.Add(this._btnNuevo);
            this._panelBotones.Controls.Add(this._btnEditar);
            this._panelBotones.Controls.Add(this._btnEliminar);
            this._panelBotones.Controls.Add(this._btnRefrescar);
            //
            // FrmEntregas
            //
            this.Text = "Entregas";
            this.Width = 820;
            this.Height = 500;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Controls.Add(this._grilla);
            this.Controls.Add(this._panelBotones);
            this.Name = "FrmEntregas";
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView _grilla;
        private System.Windows.Forms.FlowLayoutPanel _panelBotones;
        private System.Windows.Forms.Button _btnNuevo;
        private System.Windows.Forms.Button _btnEditar;
        private System.Windows.Forms.Button _btnEliminar;
        private System.Windows.Forms.Button _btnRefrescar;
    }
}
