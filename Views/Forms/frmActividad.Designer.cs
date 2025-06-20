using System;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;

namespace ClubDeportivo.Views.Forms
    {
        partial class frmActividad
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
                this.lblNombre = new System.Windows.Forms.Label();
                this.txtNombre = new System.Windows.Forms.TextBox();
                this.lblDescripcion = new System.Windows.Forms.Label();
                this.txtDescripcion = new System.Windows.Forms.TextBox();
                this.lblHorario = new System.Windows.Forms.Label();
                this.txtHorario = new System.Windows.Forms.TextBox();
                this.lblPrecio = new System.Windows.Forms.Label();
                this.txtPrecio = new System.Windows.Forms.NumericUpDown();
                this.lblExclusiva = new System.Windows.Forms.Label();
                this.chkExclusiva = new System.Windows.Forms.CheckBox();
                this.btnGuardar = new System.Windows.Forms.Button();
                this.btnCancelar = new System.Windows.Forms.Button();
                ((System.ComponentModel.ISupportInitialize)(this.txtPrecio)).BeginInit();
                this.SuspendLayout();
                // 
                // lblNombre
                // 
                this.lblNombre.AutoSize = true;
                this.lblNombre.Location = new System.Drawing.Point(12, 15);
                this.lblNombre.Name = "lblNombre";
                this.lblNombre.Size = new System.Drawing.Size(47, 13);
                this.lblNombre.TabIndex = 0;
                this.lblNombre.Text = "Nombre:";
                // 
                // txtNombre
                // 
                this.txtNombre.Location = new System.Drawing.Point(100, 12);
                this.txtNombre.Name = "txtNombre";
                this.txtNombre.Size = new System.Drawing.Size(250, 20);
                this.txtNombre.TabIndex = 1;
                // 
                // lblDescripcion
                // 
                this.lblDescripcion.AutoSize = true;
                this.lblDescripcion.Location = new System.Drawing.Point(12, 41);
                this.lblDescripcion.Name = "lblDescripcion";
                this.lblDescripcion.Size = new System.Drawing.Size(66, 13);
                this.lblDescripcion.TabIndex = 2;
                this.lblDescripcion.Text = "Descripción:";
                // 
                // txtDescripcion
                // 
                this.txtDescripcion.Location = new System.Drawing.Point(100, 38);
                this.txtDescripcion.Name = "txtDescripcion";
                this.txtDescripcion.Size = new System.Drawing.Size(250, 20);
                this.txtDescripcion.TabIndex = 3;
                // 
                // lblHorario
                // 
                this.lblHorario.AutoSize = true;
                this.lblHorario.Location = new System.Drawing.Point(12, 67);
                this.lblHorario.Name = "lblHorario";
                this.lblHorario.Size = new System.Drawing.Size(44, 13);
                this.lblHorario.TabIndex = 4;
                this.lblHorario.Text = "Horario:";
                // 
                // txtHorario
                // 
                this.txtHorario.Location = new System.Drawing.Point(100, 64);
                this.txtHorario.Name = "txtHorario";
                this.txtHorario.Size = new System.Drawing.Size(250, 20);
                this.txtHorario.TabIndex = 5;
                // 
                // lblPrecio
                // 
                this.lblPrecio.AutoSize = true;
                this.lblPrecio.Location = new System.Drawing.Point(12, 93);
                this.lblPrecio.Name = "lblPrecio";
                this.lblPrecio.Size = new System.Drawing.Size(40, 13);
                this.lblPrecio.TabIndex = 6;
                this.lblPrecio.Text = "Precio:";
                // 
                // txtPrecio
                // 
                this.txtPrecio.DecimalPlaces = 2;
                this.txtPrecio.Location = new System.Drawing.Point(100, 90);
                this.txtPrecio.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
                this.txtPrecio.Name = "txtPrecio";
                this.txtPrecio.Size = new System.Drawing.Size(100, 20);
                this.txtPrecio.TabIndex = 7;
                this.txtPrecio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                // 
                // lblExclusiva
                // 
                this.lblExclusiva.AutoSize = true;
                this.lblExclusiva.Location = new System.Drawing.Point(12, 119);
                this.lblExclusiva.Name = "lblExclusiva";
                this.lblExclusiva.Size = new System.Drawing.Size(82, 13);
                this.lblExclusiva.TabIndex = 8;
                this.lblExclusiva.Text = "Exclusiva socios:";
                // 
                // chkExclusiva
                // 
                this.chkExclusiva.AutoSize = true;
                this.chkExclusiva.Location = new System.Drawing.Point(100, 119);
                this.chkExclusiva.Name = "chkExclusiva";
                this.chkExclusiva.Size = new System.Drawing.Size(15, 14);
                this.chkExclusiva.TabIndex = 9;
                this.chkExclusiva.UseVisualStyleBackColor = true;
                // 
                // btnGuardar
                // 
                this.btnGuardar.Location = new System.Drawing.Point(194, 150);
                this.btnGuardar.Name = "btnGuardar";
                this.btnGuardar.Size = new System.Drawing.Size(75, 23);
                this.btnGuardar.TabIndex = 10;
                this.btnGuardar.Text = "Guardar";
                this.btnGuardar.UseVisualStyleBackColor = true;
                this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
                // 
                // btnCancelar
                // 
                this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.btnCancelar.Location = new System.Drawing.Point(275, 150);
                this.btnCancelar.Name = "btnCancelar";
                this.btnCancelar.Size = new System.Drawing.Size(75, 23);
                this.btnCancelar.TabIndex = 11;
                this.btnCancelar.Text = "Cancelar";
                this.btnCancelar.UseVisualStyleBackColor = true;
                // 
                // frmActividad
                // 
                this.AcceptButton = this.btnGuardar;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.CancelButton = this.btnCancelar;
                this.ClientSize = new System.Drawing.Size(362, 185);
                this.Controls.Add(this.btnCancelar);
                this.Controls.Add(this.btnGuardar);
                this.Controls.Add(this.chkExclusiva);
                this.Controls.Add(this.lblExclusiva);
                this.Controls.Add(this.txtPrecio);
                this.Controls.Add(this.lblPrecio);
                this.Controls.Add(this.txtHorario);
                this.Controls.Add(this.lblHorario);
                this.Controls.Add(this.txtDescripcion);
                this.Controls.Add(this.lblDescripcion);
                this.Controls.Add(this.txtNombre);
                this.Controls.Add(this.lblNombre);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "frmActividad";
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Text = "Nueva Actividad";
                ((System.ComponentModel.ISupportInitialize)(this.txtPrecio)).EndInit();
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            private System.Windows.Forms.Label lblNombre;
            private System.Windows.Forms.TextBox txtNombre;
            private System.Windows.Forms.Label lblDescripcion;
            private System.Windows.Forms.TextBox txtDescripcion;
            private System.Windows.Forms.Label lblHorario;
            private System.Windows.Forms.TextBox txtHorario;
            private System.Windows.Forms.Label lblPrecio;
            private System.Windows.Forms.NumericUpDown txtPrecio;
            private System.Windows.Forms.Label lblExclusiva;
            private System.Windows.Forms.CheckBox chkExclusiva;
            private System.Windows.Forms.Button btnGuardar;
            private System.Windows.Forms.Button btnCancelar;
        }
    }
