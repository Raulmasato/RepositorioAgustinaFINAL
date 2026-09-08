namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmBackup
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
            this._btnGenerar = new System.Windows.Forms.Button();
            this._btnRestaurar = new System.Windows.Forms.Button();
            this._btnRefrescar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).BeginInit();
            this.SuspendLayout();
            //
            // _grilla
            //
            this._grilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grilla.ReadOnly = true;
            this._grilla.AllowUserToAddRows = false;
            this._grilla.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grilla.AutoGenerateColumns = false;
            this._grilla.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grilla.Name = "_grilla";
            this._grilla.TabIndex = 0;
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "IdBackup", HeaderText = "Id", Width = 50, Visible = false });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "FechaHora", HeaderText = "Fecha/Hora" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "RutaArchivo", HeaderText = "Archivo" });
            this._grilla.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { DataPropertyName = "Resultado", HeaderText = "Resultado" });
            //
            // _btnGenerar
            //
            this._btnGenerar.AutoSize = true;
            this._btnGenerar.Name = "_btnGenerar";
            this._btnGenerar.TabIndex = 1;
            this._btnGenerar.Text = "Generar backup";
            this._btnGenerar.UseVisualStyleBackColor = true;
            this._btnGenerar.Click += new System.EventHandler(this.BtnGenerar_Click);
            //
            // _btnRestaurar
            //
            this._btnRestaurar.AutoSize = true;
            this._btnRestaurar.Name = "_btnRestaurar";
            this._btnRestaurar.TabIndex = 2;
            this._btnRestaurar.Text = "Restaurar";
            this._btnRestaurar.UseVisualStyleBackColor = true;
            this._btnRestaurar.Click += new System.EventHandler(this.BtnRestaurar_Click);
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
            // _panelBotones
            //
            this._panelBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelBotones.Height = 42;
            this._panelBotones.Padding = new System.Windows.Forms.Padding(6);
            this._panelBotones.Name = "_panelBotones";
            this._panelBotones.TabIndex = 4;
            this._panelBotones.Controls.Add(this._btnGenerar);
            this._panelBotones.Controls.Add(this._btnRestaurar);
            this._panelBotones.Controls.Add(this._btnRefrescar);
            //
            // FrmBackup
            //
            this.Controls.Add(this._grilla);
            this.Controls.Add(this._panelBotones);
            this.Text = "Copias de seguridad";
            this.Width = 760;
            this.Height = 480;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name = "FrmBackup";
            ((System.ComponentModel.ISupportInitialize)(this._grilla)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView _grilla;
        private System.Windows.Forms.FlowLayoutPanel _panelBotones;
        private System.Windows.Forms.Button _btnGenerar;
        private System.Windows.Forms.Button _btnRestaurar;
        private System.Windows.Forms.Button _btnRefrescar;
    }
}
