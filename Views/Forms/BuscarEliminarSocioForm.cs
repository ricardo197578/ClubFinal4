//Formulario para eliminar un socio y completar la gestion del socio por las dudas
using System;
using System.Drawing;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Services;
using ClubDeportivo.Interfaces;


namespace ClubDeportivo.Views.Forms
{
    public class BuscarEliminarSocioForm : Form
    {
        private readonly ISocioService _socioService;
        private TextBox txtBuscarDni;
        private Button btnBuscar;
        private Button btnEliminar;
        private Button btnCancelar;
        private DataGridView dgvResultados;

        public BuscarEliminarSocioForm(ISocioService socioService)
        {
            _socioService = socioService;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Buscar y Eliminar Socio por DNI";
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            txtBuscarDni = new TextBox();
            txtBuscarDni.Location = new Point(20, 20);
            txtBuscarDni.Width = 200;
            txtBuscarDni.MaxLength = 8;

            btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(230, 20);
            btnBuscar.Width = 80;
            btnBuscar.Click += BtnBuscar_Click;

            dgvResultados = new DataGridView();
            dgvResultados.Location = new Point(20, 60);
            dgvResultados.Width = 440;
            dgvResultados.Height = 150;
            dgvResultados.ReadOnly = true;
            dgvResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResultados.AllowUserToAddRows = false;
            dgvResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResultados.Columns.Add("Id", "ID");
            dgvResultados.Columns.Add("NombreCompleto", "Nombre Completo");
            dgvResultados.Columns.Add("Dni", "DNI");
            dgvResultados.Columns.Add("TipoSocio", "Tipo de Socio");

            btnEliminar = new Button();
            btnEliminar.Text = "Eliminar";
            btnEliminar.Location = new Point(150, 220);
            btnEliminar.Width = 100;
            btnEliminar.Enabled = false;
            btnEliminar.Click += BtnEliminar_Click;

            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(260, 220);
            btnCancelar.Width = 100;
            btnCancelar.Click += delegate (object sender, EventArgs e)
            {
                this.Close();
            };

            dgvResultados.SelectionChanged += delegate (object sender, EventArgs e)
            {
                btnEliminar.Enabled = dgvResultados.SelectedRows.Count > 0;
            };

            this.Controls.AddRange(new Control[]
            {
                txtBuscarDni,
                btnBuscar,
                dgvResultados,
                btnEliminar,
                btnCancelar
            });
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarDni.Text))
            {
                MessageBox.Show("Ingrese un DNI para buscar", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var socio = _socioService.GetSocio(txtBuscarDni.Text);
                dgvResultados.Rows.Clear();

                if (socio != null)
                {
                    string nombreCompleto = string.Format("{0}, {1}", socio.Apellido, socio.Nombre);
                    dgvResultados.Rows.Add(
                        socio.Id,
                        nombreCompleto,
                        socio.Dni,
                        socio.Tipo.ToString());
                }
                else
                {
                    MessageBox.Show("No se encontró ningún socio con ese DNI", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar socio: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvResultados.SelectedRows.Count == 0) return;

            var idSocio = Convert.ToInt32(dgvResultados.SelectedRows[0].Cells["Id"].Value);
            var nombreCompleto = dgvResultados.SelectedRows[0].Cells["NombreCompleto"].Value.ToString();

            if (MessageBox.Show("¿Está seguro que desea eliminar al socio " + nombreCompleto + "?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _socioService.EliminarSocio(idSocio);
                    MessageBox.Show("Socio eliminado correctamente");
                    dgvResultados.Rows.Remove(dgvResultados.SelectedRows[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar socio: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
