using System;
using System.Windows.Forms;
using ClubDeportivo.Services;
using ClubDeportivo.Repositories;
using ClubDeportivo.Models;
using System.Drawing;


namespace ClubDeportivo.Views.Forms
{
    public class NoSocioForm : Form
    {
        private readonly NoSocioService _noSocioService;
        private readonly TextBox txtNombre;
        private readonly TextBox txtApellido;
        private readonly TextBox txtDni;
        private readonly DataGridView dataGridView;

        public NoSocioForm()
        {
            // Configuración inicial del formulario
            this.Text = "Gestión de No Socios";
            this.Width = 650;  
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Inicialización de dependencias
            var dbHelper = new DatabaseHelper();
            var repo = new NoSocioRepository(dbHelper);
            _noSocioService = new NoSocioService(repo);

            // Inicialización de controles
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtDni = new TextBox();
            dataGridView = new DataGridView();

            // Crear controles
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Configuración de los controles de entrada
            txtNombre.Location = new Point(120, 20);
            txtNombre.Size = new Size(300, 20);

            txtApellido.Location = new Point(120, 60);
            txtApellido.Size = new Size(300, 20);

            txtDni.Location = new Point(120, 100);
            txtDni.Size = new Size(300, 20);

            // Configuración del DataGridView
            dataGridView.Location = new Point(20, 200);
            dataGridView.Size = new Size(600, 250);
            dataGridView.Font = new Font("Microsoft Sans Serif", 9);
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.RowHeadersVisible = false;
            dataGridView.ScrollBars = ScrollBars.Vertical;

            // Configurar columnas del DataGridView
            ConfigurarColumnasDataGrid();

            // Etiquetas
            var lblNombre = new Label
            {
                Text = "Nombre:",
                Location = new Point(20, 20),
                Size = new Size(80, 20)
            };

            var lblApellido = new Label
            {
                Text = "Apellido:",
                Location = new Point(20, 60),
                Size = new Size(80, 20)
            };

            var lblDni = new Label
            {
                Text = "DNI:",
                Location = new Point(20, 100),
                Size = new Size(80, 20)
            };

            // Botones
            var btnGuardar = new Button
            {
                Text = "Registrar No Socio",
                Location = new Point(120, 140),
                Size = new Size(150, 30)
            };

            var btnListar = new Button
            {
                Text = "Listar No Socios",
                Location = new Point(280, 140),
                Size = new Size(150, 30)
            };

            var btnSalir = new Button
            {
                Text = "Salir",
                Location = new Point(440, 140),
                Size = new Size(80, 30),
                BackColor = Color.LightGray
            };

            // Configuración de eventos
            btnGuardar.Click += BtnGuardar_Click;
            btnListar.Click += BtnListar_Click;
            btnSalir.Click += (sender, e) => this.Close();

            // Agregar controles al formulario
            this.Controls.AddRange(new Control[]
            {
                lblNombre, txtNombre,
                lblApellido, txtApellido,
                lblDni, txtDni,
                btnGuardar, btnListar, btnSalir,
                dataGridView
            });
        }

        private void ConfigurarColumnasDataGrid()
        {
            dataGridView.Columns.Clear();

            // Configuración de columnas
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Id",
                HeaderText = "ID",
                Width = 50
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Nombre",
                HeaderText = "Nombre",
                Width = 150
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Apellido",
                HeaderText = "Apellido",
                Width = 150
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Dni",
                HeaderText = "DNI",
                Width = 100
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FechaRegistro",
                HeaderText = "Fecha Registro",
                Width = 120
            });
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación completa de todos los campos requeridos
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                   string.IsNullOrWhiteSpace(txtApellido.Text) ||
                   string.IsNullOrWhiteSpace(txtDni.Text))
                {
                    MessageBox.Show("Debe completar nombre, apellido y DNI", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Registrar con todos los datos
                _noSocioService.RegistrarNoSocio(txtNombre.Text, txtApellido.Text, txtDni.Text);

                MessageBox.Show("No Socio registrado exitosamente", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar campos
                txtNombre.Clear();
                txtApellido.Clear();
                txtDni.Clear();
                txtNombre.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al registrar: {0}", ex.Message),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnListar_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView.Rows.Clear();
                var noSocios = _noSocioService.ObtenerNoSocios();

                foreach (var ns in noSocios)
                {
                    dataGridView.Rows.Add(
                        ns.Id,
                        ns.Nombre,
                        ns.Apellido,
                        ns.Dni,
                        ns.FechaRegistro.ToShortDateString()
                    );
                }

                // Mostrar conteo de registros
                this.Text = string.Format("Gestión de No Socios - Mostrando {0} registros", dataGridView.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al listar: {0}", ex.Message),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}