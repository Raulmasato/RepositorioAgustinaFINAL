namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmReservaEditar
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
            this._lblVencimiento = new System.Windows.Forms.Label();
            this._dtpVencimiento = new System.Windows.Forms.DateTimePicker();
            this._lblEstado = new System.Windows.Forms.Label();
            this._cmbEstado = new System.Windows.Forms.ComboBox();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
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
            // _lblVencimiento
            //
            this._lblVencimiento.Location = new System.Drawing.Point(20, 90);
            this._lblVencimiento.Name = "_lblVencimiento";
            this._lblVencimiento.Size = new System.Drawing.Size(100, 23);
            this._lblVencimiento.TabIndex = 4;
            this._lblVencimiento.Text = "Vencimiento";
            //
            // _dtpVencimiento
            //
            this._dtpVencimiento.Location = new System.Drawing.Point(130, 87);
            this._dtpVencimiento.Name = "_dtpVencimiento";
            this._dtpVencimiento.Size = new System.Drawing.Size(220, 23);
            this._dtpVencimiento.TabIndex = 5;
            //
            // _lblEstado
            //
            this._lblEstado.Location = new System.Drawing.Point(20, 125);
            this._lblEstado.Name = "_lblEstado";
            this._lblEstado.Size = new System.Drawing.Size(100, 23);
            this._lblEstado.TabIndex = 6;
            this._lblEstado.Text = "Estado";
            //
            // _cmbEstado
            //
            this._cmbEstado.Location = new System.Drawing.Point(130, 122);
            this._cmbEstado.Name = "_cmbEstado";
            this._cmbEstado.Size = new System.Drawing.Size(220, 23);
            this._cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbEstado.TabIndex = 7;
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(130, 165);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(90, 23);
            this._btnGuardar.TabIndex = 8;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _btnCancelar
            //
            this._btnCancelar.Location = new System.Drawing.Point(230, 165);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(90, 23);
            this._btnCancelar.TabIndex = 9;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FrmReservaEditar
            //
            this.Text = "Reservas";
            this.Width = 400;
            this.Height = 250;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Controls.Add(this._lblVehiculo);
            this.Controls.Add(this._cmbVehiculo);
            this.Controls.Add(this._lblCliente);
            this.Controls.Add(this._cmbCliente);
            this.Controls.Add(this._lblVencimiento);
            this.Controls.Add(this._dtpVencimiento);
            this.Controls.Add(this._lblEstado);
            this.Controls.Add(this._cmbEstado);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.Name = "FrmReservaEditar";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblVehiculo;
        private System.Windows.Forms.ComboBox _cmbVehiculo;
        private System.Windows.Forms.Label _lblCliente;
        private System.Windows.Forms.ComboBox _cmbCliente;
        private System.Windows.Forms.Label _lblVencimiento;
        private System.Windows.Forms.DateTimePicker _dtpVencimiento;
        private System.Windows.Forms.Label _lblEstado;
        private System.Windows.Forms.ComboBox _cmbEstado;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}
