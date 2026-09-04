namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmEntregaEditar
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
            this._lblFecha = new System.Windows.Forms.Label();
            this._dtpFecha = new System.Windows.Forms.DateTimePicker();
            this._lblLugar = new System.Windows.Forms.Label();
            this._txtLugar = new System.Windows.Forms.TextBox();
            this._lblEstado = new System.Windows.Forms.Label();
            this._cmbEstado = new System.Windows.Forms.ComboBox();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
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
            // _lblFecha
            //
            this._lblFecha.Location = new System.Drawing.Point(20, 55);
            this._lblFecha.Name = "_lblFecha";
            this._lblFecha.Size = new System.Drawing.Size(100, 23);
            this._lblFecha.TabIndex = 2;
            this._lblFecha.Text = "Fecha";
            //
            // _dtpFecha
            //
            this._dtpFecha.Location = new System.Drawing.Point(130, 52);
            this._dtpFecha.Name = "_dtpFecha";
            this._dtpFecha.Size = new System.Drawing.Size(220, 23);
            this._dtpFecha.TabIndex = 3;
            //
            // _lblLugar
            //
            this._lblLugar.Location = new System.Drawing.Point(20, 90);
            this._lblLugar.Name = "_lblLugar";
            this._lblLugar.Size = new System.Drawing.Size(100, 23);
            this._lblLugar.TabIndex = 4;
            this._lblLugar.Text = "Lugar";
            //
            // _txtLugar
            //
            this._txtLugar.Location = new System.Drawing.Point(130, 87);
            this._txtLugar.Name = "_txtLugar";
            this._txtLugar.Size = new System.Drawing.Size(220, 23);
            this._txtLugar.TabIndex = 5;
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
            // FrmEntregaEditar
            //
            this.Text = "Entregas";
            this.Width = 400;
            this.Height = 250;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Controls.Add(this._lblContrato);
            this.Controls.Add(this._cmbContrato);
            this.Controls.Add(this._lblFecha);
            this.Controls.Add(this._dtpFecha);
            this.Controls.Add(this._lblLugar);
            this.Controls.Add(this._txtLugar);
            this.Controls.Add(this._lblEstado);
            this.Controls.Add(this._cmbEstado);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.Name = "FrmEntregaEditar";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblContrato;
        private System.Windows.Forms.ComboBox _cmbContrato;
        private System.Windows.Forms.Label _lblFecha;
        private System.Windows.Forms.DateTimePicker _dtpFecha;
        private System.Windows.Forms.Label _lblLugar;
        private System.Windows.Forms.TextBox _txtLugar;
        private System.Windows.Forms.Label _lblEstado;
        private System.Windows.Forms.ComboBox _cmbEstado;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}
