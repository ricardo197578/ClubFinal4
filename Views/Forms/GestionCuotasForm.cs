using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Services;
using ClubDeportivo.Interfaces;

namespace ClubDeportivo.Views.Forms
{
    public class GestionCuotasForm : Form
    {
        private readonly ICuotaService _cuotaService;  // Servicio para obtener datos de cuotas y socios
        private readonly DataGridView dataGridView;    // Control para mostrar datos tabulares

        private Button btnListarTodos;
        private Button btnSociosVencidos;
        private Button btnDetalleCuotas;
        private Button btnSalir;

        // Constructor recibe servicio de cuotas
        public GestionCuotasForm(ICuotaService cuotaService)
        {
            _cuotaService = cuotaService;

            // Configuración básica del formulario
            this.Text = "Gestión de Cuotas";
            this.Width = 800;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Crear y configurar el DataGridView para mostrar información de socios y cuotas
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(20, 80);
            dataGridView.Size = new Size(this.Width - 50, 350);
            dataGridView.Font = new Font("Microsoft Sans Serif", 9); // Fuente compatible con .NET 4.0
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.RowHeadersVisible = false;
            dataGridView.ScrollBars = ScrollBars.Vertical;

            // Configurar las columnas del DataGridView
            ConfigurarColumnasDataGrid();

            // Crear botones y asignar eventos
            InitializeButtons();

            // Añadir DataGridView al formulario
            this.Controls.Add(dataGridView);
        }

        // Define columnas visibles del DataGridView
        private void ConfigurarColumnasDataGrid()
        {
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ApellidoNombre",
                HeaderText = "Apellido y Nombre",
                Width = 200
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DNI",
                HeaderText = "DNI",
                Width = 100
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Vencimiento",
                HeaderText = "Vencimiento Cuota",
                Width = 120
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Estado",
                HeaderText = "Estado",
                Width = 80
            });

            // Estilo especial para la columna Estado: color rojo y negrita para "VENCIDO"
            dataGridView.Columns["Estado"].DefaultCellStyle.ForeColor = Color.Red;
            dataGridView.Columns["Estado"].DefaultCellStyle.Font = new Font(dataGridView.Font, FontStyle.Bold);
        }

        // Inicializa botones con texto, ubicación, tamaño y eventos
        private void InitializeButtons()
        {
            btnListarTodos = new Button();
            btnListarTodos.Text = "Listar Todos los Socios";
            btnListarTodos.Location = new Point(20, 20);
            btnListarTodos.Size = new Size(150, 30);
            btnListarTodos.Click += new EventHandler(BtnListarTodos_Click);

            btnSociosVencidos = new Button();
            btnSociosVencidos.Text = "Vencen Hoy";
            btnSociosVencidos.Location = new Point(180, 20);
            btnSociosVencidos.Size = new Size(180, 30);
            btnSociosVencidos.BackColor = Color.LightCoral;
            btnSociosVencidos.Click += new EventHandler(BtnSociosVencidos_Click);

            btnDetalleCuotas = new Button();
            btnDetalleCuotas.Text = "Vencimientos Proximos";
            btnDetalleCuotas.Location = new Point(370, 20);
            btnDetalleCuotas.Size = new Size(150, 30);
            btnDetalleCuotas.BackColor = Color.LightGreen;
            btnDetalleCuotas.Click += new EventHandler(BtnDetalleCuotas_Click);

            btnSalir = new Button();
            btnSalir.Text = "Salir";
            btnSalir.Location = new Point(530, 20);
            btnSalir.Size = new Size(80, 30);
            btnSalir.BackColor = Color.LightGray;
            btnSalir.Click += (sender, e) => this.Close();

            // Añadir botones al formulario
            this.Controls.Add(btnListarTodos);
            this.Controls.Add(btnSociosVencidos);
            this.Controls.Add(btnDetalleCuotas);
            this.Controls.Add(btnSalir);
        }

        // Evento botón listar todos los socios
        private void BtnListarTodos_Click(object sender, EventArgs e)
        {
            CargarSocios(_cuotaService.ObtenerTodosSocios());
        }

        // Evento botón listar socios con cuotas vencidas hoy
        private void BtnSociosVencidos_Click(object sender, EventArgs e)
        {
            CargarSocios(_cuotaService.ObtenerSociosConCuotasVencidas(DateTime.Today));
        }

        // Evento botón para mostrar formulario detalle cuotas (abre nuevo formulario)
        private void BtnDetalleCuotas_Click(object sender, EventArgs e)
        {
            SociosConCuotasForm detalleForm = new SociosConCuotasForm(_cuotaService);
            detalleForm.ShowDialog();
        }

        // Carga los socios en el DataGridView y resalta los vencidos
        private void CargarSocios(IEnumerable<Socio> socios)
        {
            dataGridView.Rows.Clear();

            foreach (Socio socio in socios.OrderBy(s => s.Apellido).ThenBy(s => s.Nombre))
            {
                string estadoCuota = socio.FechaVencimientoCuota < DateTime.Today ? "VENCIDO" : "AL DÍA";

                int rowIndex = dataGridView.Rows.Add(
                    string.Format("{0}, {1}", socio.Apellido, socio.Nombre),
                    socio.Dni,
                    socio.FechaVencimientoCuota.ToString("dd/MM/yyyy"),
                    estadoCuota
                );

                // Resalta las filas con cuotas vencidas con un color de fondo rosa claro
                if (estadoCuota == "VENCIDO")
                {
                    dataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                }
            }

            // Actualiza el título del formulario con la cantidad de registros mostrados
            this.Text = string.Format("Gestión de Cuotas - Mostrando {0} socios", dataGridView.Rows.Count);
        }
    }
}
