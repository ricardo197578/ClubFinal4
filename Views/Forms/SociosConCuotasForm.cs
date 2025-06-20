using System;
using System.Windows.Forms;
using ClubDeportivo.Services;
using System.Collections.Generic;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;
using System.Drawing;

namespace ClubDeportivo.Views.Forms
{
    public class SociosConCuotasForm : Form
    {
        private readonly ICuotaService _cuotaService;
        private DataGridView dataGridViewSocios;
        private Button btnFiltrarVencidos;
        private Button btnFiltrarTodos;
        private Button btnVolver;
        private DateTimePicker dtpFechaConsulta;

        public SociosConCuotasForm(ICuotaService cuotaService)
        {
            _cuotaService = cuotaService;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Socios con Cuotas";
            this.Width = 800;  
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Configuración del DataGridView
            dataGridViewSocios = new DataGridView();
            dataGridViewSocios.Location = new Point(20, 60);
            dataGridViewSocios.Size = new Size(740, 380);
            dataGridViewSocios.Font = new Font("Microsoft Sans Serif", 9);
            dataGridViewSocios.ReadOnly = true;
            dataGridViewSocios.AllowUserToAddRows = false;
            dataGridViewSocios.AllowUserToDeleteRows = false;
            dataGridViewSocios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSocios.RowHeadersVisible = false;
            dataGridViewSocios.ScrollBars = ScrollBars.Vertical;
            dataGridViewSocios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar columnas
            ConfigurarColumnasDataGrid();

            // Controles
            dtpFechaConsulta = new DateTimePicker();
            dtpFechaConsulta.Format = DateTimePickerFormat.Short;
            dtpFechaConsulta.Location = new Point(20, 20);
            dtpFechaConsulta.Size = new Size(150, 20);
            dtpFechaConsulta.Value = DateTime.Today;

            btnFiltrarVencidos = new Button();
            btnFiltrarVencidos.Text = "Filtrar Vencidos";
            btnFiltrarVencidos.Location = new Point(180, 20);
            btnFiltrarVencidos.Size = new Size(120, 30);

            btnFiltrarTodos = new Button();
            btnFiltrarTodos.Text = "Mostrar Todos";
            btnFiltrarTodos.Location = new Point(310, 20);
            btnFiltrarTodos.Size = new Size(120, 30);

            btnVolver = new Button();
            btnVolver.Text = "Volver";
            btnVolver.Location = new Point(680, 20);
            btnVolver.Size = new Size(80, 30);
            btnVolver.BackColor = Color.LightGray;

            // Eventos
            btnFiltrarVencidos.Click += new EventHandler((s, e) => FiltrarSocios(true));
            btnFiltrarTodos.Click += new EventHandler((s, e) => FiltrarSocios(false));
            btnVolver.Click += new EventHandler((s, e) => this.Close());

            // Agregar controles
            this.Controls.AddRange(new Control[] {
                dtpFechaConsulta,
                btnFiltrarVencidos,
                btnFiltrarTodos,
                btnVolver,
                dataGridViewSocios
            });
        }

        private void ConfigurarColumnasDataGrid()
        {
            dataGridViewSocios.Columns.Clear();

            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Apellido",
                HeaderText = "Apellido",
                DataPropertyName = "Apellido"
            });

            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Nombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre"
            });

            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DNI",
                HeaderText = "DNI",
                DataPropertyName = "Dni"
            });

            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Vencimiento",
                HeaderText = "Vencimiento Cuota",
                DataPropertyName = "FechaVencimientoCuota"
            });

            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Estado",
                HeaderText = "Estado",
                DataPropertyName = "EstadoActivo"
            });

            // Formato para columna de vencimiento
            dataGridViewSocios.Columns["Vencimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
            
            // Formato para columna de estado
            dataGridViewSocios.Columns["Estado"].ValueType = typeof(string);
        }

        private void FiltrarSocios(bool soloVencidos)
        {
            dataGridViewSocios.Rows.Clear();
            IEnumerable<Socio> socios;

            if (soloVencidos)
            {
                socios = _cuotaService.ObtenerSociosConCuotasVencidas(dtpFechaConsulta.Value);
            }
            else
            {
                socios = _cuotaService.ObtenerTodosSocios();
            }

            foreach (var socio in socios)
            {
                int rowIndex = dataGridViewSocios.Rows.Add(
                    socio.Apellido,
                    socio.Nombre,
                    socio.Dni,
                    socio.FechaVencimientoCuota,
                    socio.EstadoActivo ? "Activo" : "Inactivo"
                );

                // Resaltar filas vencidas
                if (soloVencidos)
                {
                    dataGridViewSocios.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                }
            }

            // Mostrar conteo de registros
            this.Text = string.Format("Socios con Cuotas - Mostrando {0} registros", dataGridViewSocios.Rows.Count);
        }
    }
}