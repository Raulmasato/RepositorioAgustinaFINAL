namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmContratoEditar
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
            this._lblVehiculo = new System.Windows.Forms.Label();
            this._cmbVehiculo = new System.Windows.Forms.ComboBox();
            this._lblCliente = new System.Windows.Forms.Label();
            this._cmbCliente = new System.Windows.Forms.ComboBox();
            this._lblPresupuesto = new System.Windows.Forms.Label();
            this._cmbPresupuesto = new System.Windows.Forms.ComboBox();
            this._lblPrecio = new System.Windows.Forms.Label();
            this._numPrecio = new System.Windows.Forms.NumericUpDown();
            this._lblEstado = new System.Windows.Forms.Label();
            this._cmbEstado = new System.Windows.Forms.ComboBox();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._numPrecio)).BeginInit();
            this.SuspendLayout();
            //
            // _lblVehiculo
            //
            this._lblVehiculo.Location = new System.Drawing.Point(20, 20);
            this._lblVehiculo.Name = "_lblVehiculo";
            this._lblVehiculo.Size = new System.Drawing.Size(100, 23);
            this._lblVehiculo.TabIndex = 0;
            this._lblVehiculo.Text = "Vehículo";
            //
            // _cmbVehiculo
            //
            this._cmbVehiculo.Location = new System.Drawing.Point(130, 17);
            this._cmbVehiculo.Name = "_cmbVehiculo";
            this._cmbVehiculo.Size = new System.Drawing.Size(220, 23);
            this._cmbVehiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbVehiculo.TabIndex = 1;
            //
            // _lblCliente
            //
            this._lblCliente.Location = new System.Drawing.Point(20, 55);
            this._lblCliente.Name = "_lblCliente";
            this._lblCliente.Size = new System.Drawing.Size(100, 23);
            this._lblCliente.TabIndex = 2;
            this._lblCliente.Text = "Cliente";
            //
            // _cmbCliente
            //
            this._cmbCliente.Location = new System.Drawing.Point(130, 52);
            this._cmbCliente.Name = "_cmbCliente";
            this._cmbCliente.Size = new System.Drawing.Size(220, 23);
            this._cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbCliente.TabIndex = 3;
            //
            // _lblPresupuesto
            //
            this._lblPresupuesto.Location = new System.Drawing.Point(20, 90);
            this._lblPresupuesto.Name = "_lblPresupuesto";
            this._lblPresupuesto.Size = new System.Drawing.Size(100, 23);
            this._lblPresupuesto.TabIndex = 4;
            this._lblPresupuesto.Text = "Presupuestos";
            //
            // _cmbPresupuesto
            //
            this._cmbPresupuesto.Location = new System.Drawing.Point(130, 87);
            this._cmbPresupuesto.Name = "_cmbPresupuesto";
            this._cmbPresupuesto.Size = new System.Drawing.Size(220, 23);
            this._cmbPresupuesto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbPresupuesto.TabIndex = 5;
            //
            // _lblPrecio
            //
            this._lblPrecio.Location = new System.Drawing.Point(20, 125);
            this._lblPrecio.Name = "_lblPrecio";
            this._lblPrecio.Size = new System.Drawing.Size(100, 23);
            this._lblPrecio.TabIndex = 6;
            this._lblPrecio.Text = "Precio";
            //
            // _numPrecio
            //
            this._numPrecio.Location = new System.Drawing.Point(130, 122);
            this._numPrecio.Name = "_numPrecio";
            this._numPrecio.Size = new System.Drawing.Size(150, 23);
            this._numPrecio.Maximum = 100_000_000;
            this._numPrecio.DecimalPlaces = 2;
            this._numPrecio.TabIndex = 7;
            //
            // _lblEstado
            //
            this._lblEstado.Location = new System.Drawing.Point(20, 160);
            this._lblEstado.Name = "_lblEstado";
            this._lblEstado.Size = new System.Drawing.Size(100, 23);
            this._lblEstado.TabIndex = 8;
            this._lblEstado.Text = "Estado";
            //
            // _cmbEstado
            //
            this._cmbEstado.Location = new System.Drawing.Point(130, 157);
            this._cmbEstado.Name = "_cmbEstado";
            this._cmbEstado.Size = new System.Drawing.Size(220, 23);
            this._cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbEstado.TabIndex = 9;
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(130, 200);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(90, 23);
            this._btnGuardar.TabIndex = 10;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _btnCancelar
            //
            this._btnCancelar.Location = new System.Drawing.Point(230, 200);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(90, 23);
            this._btnCancelar.TabIndex = 11;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FrmContratoEditar
            //
            this.Text = "Contratos";
            this.Width = 400;
            this.Height = 290;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Controls.Add(this._lblVehiculo);
            this.Controls.Add(this._cmbVehiculo);
            this.Controls.Add(this._lblCliente);
            this.Controls.Add(this._cmbCliente);
            this.Controls.Add(this._lblPresupuesto);
            this.Controls.Add(this._cmbPresupuesto);
            this.Controls.Add(this._lblPrecio);
            this.Controls.Add(this._numPrecio);
            this.Controls.Add(this._lblEstado);
            this.Controls.Add(this._cmbEstado);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.Name = "FrmContratoEditar";
            ((System.ComponentModel.ISupportInitialize)(this._numPrecio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblVehiculo;
        private System.Windows.Forms.ComboBox _cmbVehiculo;
        private System.Windows.Forms.Label _lblCliente;
        private System.Windows.Forms.ComboBox _cmbCliente;
        private System.Windows.Forms.Label _lblPresupuesto;
        private System.Windows.Forms.ComboBox _cmbPresupuesto;
        private System.Windows.Forms.Label _lblPrecio;
        private System.Windows.Forms.NumericUpDown _numPrecio;
        private System.Windows.Forms.Label _lblEstado;
        private System.Windows.Forms.ComboBox _cmbEstado;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}
