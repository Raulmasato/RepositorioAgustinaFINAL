namespace AutoVentas.UI.Formularios.Vendedor
{
    partial class FrmVehiculoEditar
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
            this._lblMarca = new System.Windows.Forms.Label();
            this._txtMarca = new System.Windows.Forms.TextBox();
            this._lblModelo = new System.Windows.Forms.Label();
            this._txtModelo = new System.Windows.Forms.TextBox();
            this._lblColor = new System.Windows.Forms.Label();
            this._txtColor = new System.Windows.Forms.TextBox();
            this._lblAnio = new System.Windows.Forms.Label();
            this._numAnio = new System.Windows.Forms.NumericUpDown();
            this._lblPrecio = new System.Windows.Forms.Label();
            this._numPrecio = new System.Windows.Forms.NumericUpDown();
            this._chkDisponible = new System.Windows.Forms.CheckBox();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._numAnio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numPrecio)).BeginInit();
            this.SuspendLayout();
            //
            // _lblMarca
            //
            this._lblMarca.Location = new System.Drawing.Point(20, 20);
            this._lblMarca.Name = "_lblMarca";
            this._lblMarca.Size = new System.Drawing.Size(100, 23);
            this._lblMarca.TabIndex = 0;
            this._lblMarca.Text = "Marca";
            //
            // _txtMarca
            //
            this._txtMarca.Location = new System.Drawing.Point(130, 17);
            this._txtMarca.Name = "_txtMarca";
            this._txtMarca.Size = new System.Drawing.Size(200, 23);
            this._txtMarca.TabIndex = 1;
            //
            // _lblModelo
            //
            this._lblModelo.Location = new System.Drawing.Point(20, 55);
            this._lblModelo.Name = "_lblModelo";
            this._lblModelo.Size = new System.Drawing.Size(100, 23);
            this._lblModelo.TabIndex = 2;
            this._lblModelo.Text = "Modelo";
            //
            // _txtModelo
            //
            this._txtModelo.Location = new System.Drawing.Point(130, 52);
            this._txtModelo.Name = "_txtModelo";
            this._txtModelo.Size = new System.Drawing.Size(200, 23);
            this._txtModelo.TabIndex = 3;
            //
            // _lblColor
            //
            this._lblColor.Location = new System.Drawing.Point(20, 90);
            this._lblColor.Name = "_lblColor";
            this._lblColor.Size = new System.Drawing.Size(100, 23);
            this._lblColor.TabIndex = 4;
            this._lblColor.Text = "Color";
            //
            // _txtColor
            //
            this._txtColor.Location = new System.Drawing.Point(130, 87);
            this._txtColor.Name = "_txtColor";
            this._txtColor.Size = new System.Drawing.Size(200, 23);
            this._txtColor.TabIndex = 5;
            //
            // _lblAnio
            //
            this._lblAnio.Location = new System.Drawing.Point(20, 125);
            this._lblAnio.Name = "_lblAnio";
            this._lblAnio.Size = new System.Drawing.Size(100, 23);
            this._lblAnio.TabIndex = 6;
            this._lblAnio.Text = "Año";
            //
            // _numAnio
            //
            this._numAnio.Location = new System.Drawing.Point(130, 122);
            this._numAnio.Name = "_numAnio";
            this._numAnio.Size = new System.Drawing.Size(100, 23);
            this._numAnio.TabIndex = 7;
            this._numAnio.Minimum = 1950;
            this._numAnio.Maximum = 2100;
            this._numAnio.Value = System.DateTime.Now.Year;
            //
            // _lblPrecio
            //
            this._lblPrecio.Location = new System.Drawing.Point(20, 160);
            this._lblPrecio.Name = "_lblPrecio";
            this._lblPrecio.Size = new System.Drawing.Size(100, 23);
            this._lblPrecio.TabIndex = 8;
            this._lblPrecio.Text = "Precio";
            //
            // _numPrecio
            //
            this._numPrecio.Location = new System.Drawing.Point(130, 157);
            this._numPrecio.Name = "_numPrecio";
            this._numPrecio.Size = new System.Drawing.Size(150, 23);
            this._numPrecio.TabIndex = 9;
            this._numPrecio.Maximum = 100000000;
            this._numPrecio.DecimalPlaces = 2;
            //
            // _chkDisponible
            //
            this._chkDisponible.Location = new System.Drawing.Point(130, 195);
            this._chkDisponible.Name = "_chkDisponible";
            this._chkDisponible.Size = new System.Drawing.Size(200, 23);
            this._chkDisponible.TabIndex = 10;
            this._chkDisponible.Checked = true;
            this._chkDisponible.Text = "Disponible";
            this._chkDisponible.UseVisualStyleBackColor = true;
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(130, 235);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(90, 23);
            this._btnGuardar.TabIndex = 11;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _btnCancelar
            //
            this._btnCancelar.Location = new System.Drawing.Point(230, 235);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(90, 23);
            this._btnCancelar.TabIndex = 12;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FrmVehiculoEditar
            //
            this.Text = "Vehículos";
            this.Width = 380;
            this.Height = 320;
            this.Controls.Add(this._lblMarca);
            this.Controls.Add(this._txtMarca);
            this.Controls.Add(this._lblModelo);
            this.Controls.Add(this._txtModelo);
            this.Controls.Add(this._lblColor);
            this.Controls.Add(this._txtColor);
            this.Controls.Add(this._lblAnio);
            this.Controls.Add(this._numAnio);
            this.Controls.Add(this._lblPrecio);
            this.Controls.Add(this._numPrecio);
            this.Controls.Add(this._chkDisponible);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVehiculoEditar";
            ((System.ComponentModel.ISupportInitialize)(this._numAnio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numPrecio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblMarca;
        private System.Windows.Forms.TextBox _txtMarca;
        private System.Windows.Forms.Label _lblModelo;
        private System.Windows.Forms.TextBox _txtModelo;
        private System.Windows.Forms.Label _lblColor;
        private System.Windows.Forms.TextBox _txtColor;
        private System.Windows.Forms.Label _lblAnio;
        private System.Windows.Forms.NumericUpDown _numAnio;
        private System.Windows.Forms.Label _lblPrecio;
        private System.Windows.Forms.NumericUpDown _numPrecio;
        private System.Windows.Forms.CheckBox _chkDisponible;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}
