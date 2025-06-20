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
        private readonly ICuotaService _cuotaService;       // Servicio para manejar cuotas
        private DataGridView dataGridViewSocios;             // Tabla para mostrar socios y cuotas
        private Button btnFiltrarVencidos;                    // Botón para filtrar socios con cuotas vencidas
        private Button btnFiltrarTodos;                       // Botón para mostrar todos los socios
        private Button btnVolver;                              // Botón para cerrar el formulario
        private DateTimePicker dtpFechaConsulta;              // Selector de fecha para consulta de vencidos

        public SociosConCuotasForm(ICuotaService cuotaService)
        {
            _cuotaService = cuotaService;                      // Inyección de dependencia del servicio de cuotas
            InitializeComponents();                             // Inicializar controles de interfaz
        }

        private void InitializeComponents()
        {
            this.Text = "Socios con Cuotas";                   // Título del formulario
            this.Width = 800;                                   // Ancho del formulario
            this.Height = 500;                                  // Alto del formulario
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar en pantalla
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // No redimensionable
            this.MaximizeBox = false;                           // Sin botón maximizar

            // Configuración del DataGridView para mostrar socios
            dataGridViewSocios = new DataGridView();
            dataGridViewSocios.Location = new Point(20, 60);   // Posición dentro del formulario
            dataGridViewSocios.Size = new Size(740, 380);      // Tamaño de la tabla
            dataGridViewSocios.Font = new Font("Microsoft Sans Serif", 9); // Fuente
            dataGridViewSocios.ReadOnly = true;                 // No editable
            dataGridViewSocios.AllowUserToAddRows = false;      // No permitir agregar filas manualmente
            dataGridViewSocios.AllowUserToDeleteRows = false;   // No permitir borrar filas manualmente
            dataGridViewSocios.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Selección completa por fila
            dataGridViewSocios.RowHeadersVisible = false;       // Ocultar encabezados de fila
            dataGridViewSocios.ScrollBars = ScrollBars.Vertical; // Scroll vertical
            dataGridViewSocios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ajuste automático columnas

            // Configurar columnas de la tabla
            ConfigurarColumnasDataGrid();

            // Selector de fecha para consultar cuotas vencidas
            dtpFechaConsulta = new DateTimePicker();
            dtpFechaConsulta.Format = DateTimePickerFormat.Short;  // Formato corto de fecha
            dtpFechaConsulta.Location = new Point(20, 20);          // Posición
            dtpFechaConsulta.Size = new Size(150, 20);              // Tamaño
            dtpFechaConsulta.Value = DateTime.Today;                // Fecha inicial hoy

            // Botón para filtrar socios con cuotas vencidas
            btnFiltrarVencidos = new Button();
            btnFiltrarVencidos.Text = "Filtrar Vencidos";
            btnFiltrarVencidos.Location = new Point(180, 20);
            btnFiltrarVencidos.Size = new Size(120, 30);

            // Botón para mostrar todos los socios
            btnFiltrarTodos = new Button();
            btnFiltrarTodos.Text = "Mostrar Todos";
            btnFiltrarTodos.Location = new Point(310, 20);
            btnFiltrarTodos.Size = new Size(120, 30);

            // Botón para volver (cerrar formulario)
            btnVolver = new Button();
            btnVolver.Text = "Volver";
            btnVolver.Location = new Point(680, 20);
            btnVolver.Size = new Size(80, 30);
            btnVolver.BackColor = Color.LightGray;

            // Asociar eventos a los botones
            btnFiltrarVencidos.Click += new EventHandler((s, e) => FiltrarSocios(true));
            btnFiltrarTodos.Click += new EventHandler((s, e) => FiltrarSocios(false));
            btnVolver.Click += new EventHandler((s, e) => this.Close());

            // Agregar controles al formulario
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
            dataGridViewSocios.Columns.Clear();                   // Limpiar columnas previas

            // Columna Apellido
            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Apellido",
                HeaderText = "Apellido",
                DataPropertyName = "Apellido"                     // Nombre propiedad para binding
            });

            // Columna Nombre
            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Nombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre"
            });

            // Columna DNI
            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DNI",
                HeaderText = "DNI",
                DataPropertyName = "Dni"
            });

            // Columna Fecha de Vencimiento de cuota
            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Vencimiento",
                HeaderText = "Vencimiento Cuota",
                DataPropertyName = "FechaVencimientoCuota"
            });

            // Columna Estado (Activo/Inactivo)
            dataGridViewSocios.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Estado",
                HeaderText = "Estado",
                DataPropertyName = "EstadoActivo"
            });

            // Formatear columna de vencimiento como fecha dd/MM/yyyy
            dataGridViewSocios.Columns["Vencimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";

            // Definir tipo de dato para columna Estado como string (activo/inactivo)
            dataGridViewSocios.Columns["Estado"].ValueType = typeof(string);
        }

        private void FiltrarSocios(bool soloVencidos)
        {
            dataGridViewSocios.Rows.Clear();                    // Limpiar filas existentes
            IEnumerable<Socio> socios;

            if (soloVencidos)
            {
                // Obtener socios con cuotas vencidas según fecha seleccionada
                socios = _cuotaService.ObtenerSociosConCuotasVencidas(dtpFechaConsulta.Value);
            }
            else
            {
                // Obtener todos los socios sin filtro
                socios = _cuotaService.ObtenerTodosSocios();
            }

            // Agregar filas a DataGridView para cada socio
            foreach (var socio in socios)
            {
                int rowIndex = dataGridViewSocios.Rows.Add(
                    socio.Apellido,
                    socio.Nombre,
                    socio.Dni,
                    socio.FechaVencimientoCuota,
                    socio.EstadoActivo ? "Activo" : "Inactivo"  // Mostrar estado como texto
                );

                // Si filtro es solo vencidos, resaltar fila con color
                if (soloVencidos)
                {
                    dataGridViewSocios.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                }
            }

            // Actualizar título del formulario con cantidad de registros mostrados
            this.Text = string.Format("Socios con Cuotas - Mostrando {0} registros", dataGridViewSocios.Rows.Count);
        }
    }
}
