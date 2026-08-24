namespace AutoVentas.UI.Formularios.Ejecutivo
{
    partial class FrmReporteEditar
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
            this._lblTitulo = new System.Windows.Forms.Label();
            this._txtTitulo = new System.Windows.Forms.TextBox();
            this._lblTipo = new System.Windows.Forms.Label();
            this._cmbTipo = new System.Windows.Forms.ComboBox();
            this._lblDesde = new System.Windows.Forms.Label();
            this._dtpDesde = new System.Windows.Forms.DateTimePicker();
            this._lblHasta = new System.Windows.Forms.Label();
            this._dtpHasta = new System.Windows.Forms.DateTimePicker();
            this._txtContenido = new System.Windows.Forms.TextBox();
            this._btnGuardar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _lblTitulo
            //
            this._lblTitulo.Location = new System.Drawing.Point(20, 20);
            this._lblTitulo.Name = "_lblTitulo";
            this._lblTitulo.Size = new System.Drawing.Size(100, 23);
            this._lblTitulo.TabIndex = 0;
            this._lblTitulo.Text = "Título";
            //
            // _txtTitulo
            //
            this._txtTitulo.Location = new System.Drawing.Point(130, 17);
            this._txtTitulo.Name = "_txtTitulo";
            this._txtTitulo.Size = new System.Drawing.Size(300, 23);
            this._txtTitulo.TabIndex = 1;
            //
            // _lblTipo
            //
            this._lblTipo.Location = new System.Drawing.Point(20, 55);
            this._lblTipo.Name = "_lblTipo";
            this._lblTipo.Size = new System.Drawing.Size(100, 23);
            this._lblTipo.TabIndex = 2;
            this._lblTipo.Text = "Tipo";
            //
            // _cmbTipo
            //
            this._cmbTipo.Location = new System.Drawing.Point(130, 52);
            this._cmbTipo.Name = "_cmbTipo";
            this._cmbTipo.Size = new System.Drawing.Size(200, 23);
            this._cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTipo.TabIndex = 3;
            //
            // _lblDesde
            //
            this._lblDesde.Location = new System.Drawing.Point(20, 90);
            this._lblDesde.Name = "_lblDesde";
            this._lblDesde.Size = new System.Drawing.Size(100, 23);
            this._lblDesde.TabIndex = 4;
            this._lblDesde.Text = "Desde";
            //
            // _dtpDesde
            //
            this._dtpDesde.Location = new System.Drawing.Point(130, 87);
            this._dtpDesde.Name = "_dtpDesde";
            this._dtpDesde.Size = new System.Drawing.Size(200, 23);
            this._dtpDesde.TabIndex = 5;
            //
            // _lblHasta
            //
            this._lblHasta.Location = new System.Drawing.Point(20, 125);
            this._lblHasta.Name = "_lblHasta";
            this._lblHasta.Size = new System.Drawing.Size(100, 23);
            this._lblHasta.TabIndex = 6;
            this._lblHasta.Text = "Hasta";
            //
            // _dtpHasta
            //
            this._dtpHasta.Location = new System.Drawing.Point(130, 122);
            this._dtpHasta.Name = "_dtpHasta";
            this._dtpHasta.Size = new System.Drawing.Size(200, 23);
            this._dtpHasta.TabIndex = 7;
            //
            // _txtContenido
            //
            this._txtContenido.Location = new System.Drawing.Point(20, 160);
            this._txtContenido.Name = "_txtContenido";
            this._txtContenido.Size = new System.Drawing.Size(410, 150);
            this._txtContenido.Multiline = true;
            this._txtContenido.ReadOnly = true;
            this._txtContenido.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtContenido.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 8);
            this._txtContenido.TabIndex = 8;
            //
            // _btnGuardar
            //
            this._btnGuardar.Location = new System.Drawing.Point(260, 320);
            this._btnGuardar.Name = "_btnGuardar";
            this._btnGuardar.Size = new System.Drawing.Size(90, 23);
            this._btnGuardar.TabIndex = 9;
            this._btnGuardar.Text = "Guardar";
            this._btnGuardar.UseVisualStyleBackColor = true;
            this._btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // _btnCancelar
            //
            this._btnCancelar.Location = new System.Drawing.Point(360, 320);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(90, 23);
            this._btnCancelar.TabIndex = 10;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FrmReporteEditar
            //
            this.Text = "Reportes";
            this.Width = 460;
            this.Height = 400;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Controls.Add(this._lblTitulo);
            this.Controls.Add(this._txtTitulo);
            this.Controls.Add(this._lblTipo);
            this.Controls.Add(this._cmbTipo);
            this.Controls.Add(this._lblDesde);
            this.Controls.Add(this._dtpDesde);
            this.Controls.Add(this._lblHasta);
            this.Controls.Add(this._dtpHasta);
            this.Controls.Add(this._txtContenido);
            this.Controls.Add(this._btnGuardar);
            this.Controls.Add(this._btnCancelar);
            this.Name = "FrmReporteEditar";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblTitulo;
        private System.Windows.Forms.TextBox _txtTitulo;
        private System.Windows.Forms.Label _lblTipo;
        private System.Windows.Forms.ComboBox _cmbTipo;
        private System.Windows.Forms.Label _lblDesde;
        private System.Windows.Forms.DateTimePicker _dtpDesde;
        private System.Windows.Forms.Label _lblHasta;
        private System.Windows.Forms.DateTimePicker _dtpHasta;
        private System.Windows.Forms.TextBox _txtContenido;
        private System.Windows.Forms.Button _btnGuardar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}
