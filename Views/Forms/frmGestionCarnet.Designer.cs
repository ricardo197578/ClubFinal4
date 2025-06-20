using System;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Services;

namespace ClubDeportivo.Views.Forms
{
    partial class frmGestionCarnet
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblDni = new System.Windows.Forms.Label();
            this.txtDniSocio = new System.Windows.Forms.TextBox();
            this.btnBuscarPorDni = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDniValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblApellido = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnConfirmarEntrega = new System.Windows.Forms.Button();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.lblFechaVencimiento = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblFechaEmision = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblNroCarnet = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkAptoFisico = new System.Windows.Forms.CheckBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();

            // lblDni
            this.lblDni.AutoSize = true;
            this.lblDni.Location = new System.Drawing.Point(12, 15);
            this.lblDni.Name = "lblDni";
            this.lblDni.Size = new System.Drawing.Size(29, 13);
            this.lblDni.TabIndex = 0;
            this.lblDni.Text = "DNI:";

            // txtDniSocio
            this.txtDniSocio.Location = new System.Drawing.Point(47, 12);
            this.txtDniSocio.Name = "txtDniSocio";
            this.txtDniSocio.Size = new System.Drawing.Size(100, 20);
            this.txtDniSocio.TabIndex = 1;

            // btnBuscarPorDni
            this.btnBuscarPorDni.Location = new System.Drawing.Point(153, 10);
            this.btnBuscarPorDni.Name = "btnBuscarPorDni";
            this.btnBuscarPorDni.Size = new System.Drawing.Size(75, 23);
            this.btnBuscarPorDni.TabIndex = 2;
            this.btnBuscarPorDni.Text = "Buscar";
            this.btnBuscarPorDni.UseVisualStyleBackColor = true;
            this.btnBuscarPorDni.Click += new System.EventHandler(this.btnBuscarPorDni_Click);

            // groupBox1
            this.groupBox1.Controls.Add(this.lblDniValue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblApellido);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblNombre);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(15, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos del Socio";

            // lblDniValue
            this.lblDniValue.AutoSize = true;
            this.lblDniValue.Location = new System.Drawing.Point(80, 65);
            this.lblDniValue.Name = "lblDniValue";
            this.lblDniValue.Size = new System.Drawing.Size(0, 13);
            this.lblDniValue.TabIndex = 5;

            // label3
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Documento:";

            // lblApellido
            this.lblApellido.AutoSize = true;
            this.lblApellido.Location = new System.Drawing.Point(60, 40);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(0, 13);
            this.lblApellido.TabIndex = 3;

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Apellido:";

            // lblNombre
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(60, 20);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(0, 13);
            this.lblNombre.TabIndex = 1;

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre:";

            // groupBox2
            this.groupBox2.Controls.Add(this.btnConfirmarEntrega);
            this.groupBox2.Controls.Add(this.btnGenerar);
            this.groupBox2.Controls.Add(this.lblFechaVencimiento);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblFechaEmision);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblNroCarnet);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.chkAptoFisico);
            this.groupBox2.Location = new System.Drawing.Point(15, 160);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(300, 180);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos del Carnet";

            // btnConfirmarEntrega
            this.btnConfirmarEntrega.Enabled = false;
            this.btnConfirmarEntrega.Location = new System.Drawing.Point(150, 140);
            this.btnConfirmarEntrega.Name = "btnConfirmarEntrega";
            this.btnConfirmarEntrega.Size = new System.Drawing.Size(120, 30);
            this.btnConfirmarEntrega.TabIndex = 8;
            this.btnConfirmarEntrega.Text = "Confirmar Entrega";
            this.btnConfirmarEntrega.UseVisualStyleBackColor = true;
            this.btnConfirmarEntrega.Click += new System.EventHandler(this.btnConfirmarEntrega_Click);

            // btnGenerar
            this.btnGenerar.Enabled = false;
            this.btnGenerar.Location = new System.Drawing.Point(20, 140);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(120, 30);
            this.btnGenerar.TabIndex = 7;
            this.btnGenerar.Text = "Generar Carnet";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);

            // lblFechaVencimiento
            this.lblFechaVencimiento.AutoSize = true;
            this.lblFechaVencimiento.Location = new System.Drawing.Point(120, 90);
            this.lblFechaVencimiento.Name = "lblFechaVencimiento";
            this.lblFechaVencimiento.Size = new System.Drawing.Size(0, 13);
            this.lblFechaVencimiento.TabIndex = 6;

            // label7
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Fecha Vencimiento:";

            // lblFechaEmision
            this.lblFechaEmision.AutoSize = true;
            this.lblFechaEmision.Location = new System.Drawing.Point(90, 65);
            this.lblFechaEmision.Name = "lblFechaEmision";
            this.lblFechaEmision.Size = new System.Drawing.Size(0, 13);
            this.lblFechaEmision.TabIndex = 4;

            // label5
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Fecha Emisión:";

            // lblNroCarnet
            this.lblNroCarnet.AutoSize = true;
            this.lblNroCarnet.Location = new System.Drawing.Point(80, 40);
            this.lblNroCarnet.Name = "lblNroCarnet";
            this.lblNroCarnet.Size = new System.Drawing.Size(0, 13);
            this.lblNroCarnet.TabIndex = 2;

            // label4
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "N° Carnet:";

            // chkAptoFisico
            this.chkAptoFisico.AutoSize = true;
            this.chkAptoFisico.Location = new System.Drawing.Point(10, 20);
            this.chkAptoFisico.Name = "chkAptoFisico";
            this.chkAptoFisico.Size = new System.Drawing.Size(79, 17);
            this.chkAptoFisico.TabIndex = 0;
            this.chkAptoFisico.Text = "Apto Físico";
            this.chkAptoFisico.UseVisualStyleBackColor = true;

            // btnCancelar
            this.btnCancelar.Location = new System.Drawing.Point(240, 350);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // frmGestionCarnet
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 380);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBuscarPorDni);
            this.Controls.Add(this.txtDniSocio);
            this.Controls.Add(this.lblDni);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGestionCarnet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Carnets";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblDni;
        private System.Windows.Forms.TextBox txtDniSocio;
        private System.Windows.Forms.Button btnBuscarPorDni;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDniValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnConfirmarEntrega;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Label lblFechaVencimiento;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblFechaEmision;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblNroCarnet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAptoFisico;
        private System.Windows.Forms.Button btnCancelar;
    }
}