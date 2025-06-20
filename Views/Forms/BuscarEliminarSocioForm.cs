// Formulario para buscar y eliminar un socio mediante su DNI
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
        // Servicio para manejar la lógica de socios
        private readonly ISocioService _socioService;

        // Controles del formulario
        private TextBox txtBuscarDni;
        private Button btnBuscar;
        private Button btnEliminar;
        private Button btnCancelar;
        private DataGridView dgvResultados;

        // Constructor que recibe el servicio de socios
        public BuscarEliminarSocioForm(ISocioService socioService)
        {
            _socioService = socioService;
            InitializeComponents();
        }

        // Método para configurar y agregar los controles al formulario
        private void InitializeComponents()
        {
            this.Text = "Buscar y Eliminar Socio por DNI";
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // TextBox para ingresar DNI
            txtBuscarDni = new TextBox();
            txtBuscarDni.Location = new Point(20, 20);
            txtBuscarDni.Width = 200;
            txtBuscarDni.MaxLength = 8;

            // Botón para iniciar la búsqueda
            btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(230, 20);
            btnBuscar.Width = 80;
            btnBuscar.Click += BtnBuscar_Click;

            // DataGridView para mostrar resultados de búsqueda
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

            // Botón para eliminar el socio seleccionado
            btnEliminar = new Button();
            btnEliminar.Text = "Eliminar";
            btnEliminar.Location = new Point(150, 220);
            btnEliminar.Width = 100;
            btnEliminar.Enabled = false; // Deshabilitado hasta que se seleccione una fila
            btnEliminar.Click += BtnEliminar_Click;

            // Botón para cerrar el formulario
            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(260, 220);
            btnCancelar.Width = 100;
            btnCancelar.Click += delegate (object sender, EventArgs e)
            {
                this.Close();
            };

            // Habilita o deshabilita el botón eliminar según si hay una fila seleccionada
            dgvResultados.SelectionChanged += delegate (object sender, EventArgs e)
            {
                btnEliminar.Enabled = dgvResultados.SelectedRows.Count > 0;
            };

            // Agrega los controles al formulario
            this.Controls.AddRange(new Control[]
            {
                txtBuscarDni,
                btnBuscar,
                dgvResultados,
                btnEliminar,
                btnCancelar
            });
        }

        // Evento click para buscar un socio por DNI
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
                // Busca el socio usando el servicio
                var socio = _socioService.GetSocio(txtBuscarDni.Text);
                dgvResultados.Rows.Clear();

                if (socio != null)
                {
                    // Si se encuentra, agrega una fila al DataGridView con los datos
                    string nombreCompleto = string.Format("{0}, {1}", socio.Apellido, socio.Nombre);
                    dgvResultados.Rows.Add(
                        socio.Id,
                        nombreCompleto,
                        socio.Dni,
                        socio.Tipo.ToString());
                }
                else
                {
                    // Si no se encuentra, informa al usuario
                    MessageBox.Show("No se encontró ningún socio con ese DNI", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Muestra cualquier error que ocurra al buscar
                MessageBox.Show("Error al buscar socio: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento click para eliminar el socio seleccionado
        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvResultados.SelectedRows.Count == 0) return;

            // Obtiene el Id y nombre completo del socio seleccionado
            var idSocio = Convert.ToInt32(dgvResultados.SelectedRows[0].Cells["Id"].Value);
            var nombreCompleto = dgvResultados.SelectedRows[0].Cells["NombreCompleto"].Value.ToString();

            // Pregunta confirmación al usuario antes de eliminar
            if (MessageBox.Show("¿Está seguro que desea eliminar al socio " + nombreCompleto + "?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Llama al servicio para eliminar el socio
                    _socioService.EliminarSocio(idSocio);
                    MessageBox.Show("Socio eliminado correctamente");
                    // Remueve la fila eliminada del DataGridView
                    dgvResultados.Rows.Remove(dgvResultados.SelectedRows[0]);
                }
                catch (Exception ex)
                {
                    // Muestra error en caso de fallar la eliminación
                    MessageBox.Show("Error al eliminar socio: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
