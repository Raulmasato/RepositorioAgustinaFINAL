namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmPagoEditar
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
            this._lblContrato = new System.Windows.Forms.Label();
            this._cmbContrato = new System.Windows.Forms.ComboBox();
            this._lblMonto = new System.Windows.Forms.Label();
            this._numMonto = new System.Windows.Forms.NumericUpDown();
            this._lblMetodo = new System.Windows.Forms.Label();
            this._cmbMetodo = new System.Windows.Forms.ComboBox();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._numMonto)).BeginInit();
            this.SuspendLayout();
            //
            // _lblContrato
            //
            this._lblContrato.Location = new System.Drawing.Point(20, 20);
            this._lblContrato.Name = "_lblContrato";
            this._lblContrato.Size = new System.Drawing.Size(100, 23);
            this._lblContrato.TabIndex = 0;
            this._lblContrato.Text = "Contratos";
            //
            // _cmbContrato
            //
            this._cmbContrato.Location = new System.Drawing.Point(130, 17);
            this._cmbContrato.Name = "_cmbContrato";
            this._cmbContrato.Size = new System.Drawing.Size(220, 23);
            this._cmbContrato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbContrato.TabIndex = 1;
            //
            // _lblMonto
            //
            this._lblMonto.Location = new System.Drawing.Point(20, 55);
            this._lblMonto.Name = "_lblMonto";
            this._lblMonto.Size = new System.Drawing.Size(100, 23);
            this._lblMonto.TabIndex = 2;
            this._lblMonto.Text = "Monto";
            //
            // _numMonto
            //
            this._numMonto.Location = new System.Drawing.Point(130, 52);
            this._numMonto.Name = "_numMonto";
            this._numMonto.Size = new System.Drawing.Size(150, 23);
            this._numMonto.Maximum = 100_000_000;
            this._numMonto.DecimalPlaces = 2;
            this._numMonto.TabIndex = 3;
            //
            // _lblMetodo
            //
            this._lblMetodo.Location = new System.Drawing.Point(20, 90);
            this._lblMetodo.Name = "_lblMetodo";
            this._lblMetodo.Size = new System.Drawing.Size(100, 23);
            this._lblMetodo.TabIndex = 4;
            this._lblMetodo.Text = "Método de pago";
            //
            // _cmbMetodo
            //
            this._cmbMetodo.Location = new System.Drawing.Point(130, 87);
            this._cmbMetodo.Name = "_cmbMetodo";
            this._cmbMetodo.Size = new System.Drawing.Size(220, 23);
            this._cmbMetodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbMetodo.TabIndex = 5;
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(130, 130);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(90, 23);
            this._btnGuardar.TabIndex = 6;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _btnCancelar
            //
            this._btnCancelar.Location = new System.Drawing.Point(230, 130);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(90, 23);
            this._btnCancelar.TabIndex = 7;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FrmPagoEditar
            //
            this.Text = "Pagos";
            this.Width = 400;
            this.Height = 220;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Controls.Add(this._lblContrato);
            this.Controls.Add(this._cmbContrato);
            this.Controls.Add(this._lblMonto);
            this.Controls.Add(this._numMonto);
            this.Controls.Add(this._lblMetodo);
            this.Controls.Add(this._cmbMetodo);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.Name = "FrmPagoEditar";
            ((System.ComponentModel.ISupportInitialize)(this._numMonto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblContrato;
        private System.Windows.Forms.ComboBox _cmbContrato;
        private System.Windows.Forms.Label _lblMonto;
        private System.Windows.Forms.NumericUpDown _numMonto;
        private System.Windows.Forms.Label _lblMetodo;
        private System.Windows.Forms.ComboBox _cmbMetodo;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}
