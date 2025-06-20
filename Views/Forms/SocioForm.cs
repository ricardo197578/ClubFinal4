//CODIGO COMENTADO POR IA
using System;
using System.Drawing;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;
using ClubDeportivo.Services;

namespace ClubDeportivo.Views.Forms
{
    /// <summary>
    /// Formulario para la gestión de socios del club deportivo
    /// </summary>
    public class SocioForm : Form
    {
        // Servicios inyectados para manejar la lógica de negocio
        private readonly SocioService _socioService;
        private readonly CuotaService _cuotaService;

        // Controles del formulario
        private readonly TextBox txtNombre;
        private readonly TextBox txtApellido;
        private readonly TextBox txtDni;
        private readonly DateTimePicker dtpFechaInscripcion;
        private readonly DateTimePicker dtpFechaVencimiento;
        private readonly CheckBox chkEstadoActivo;
        private readonly ComboBox cmbTipoSocio;

        /// <summary>
        /// Constructor del formulario de socios
        /// </summary>
        public SocioForm()
        {
            // Configuración básica del formulario
            this.Text = "Gestión de Socios";  // Título de la ventana
            this.Width = 450;                 // Ancho del formulario
            this.Height = 450;                // Alto del formulario
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar en pantalla

            // Inicialización de los servicios y repositorios
            var dbHelper = new DatabaseHelper();
            var socioRepo = new SocioRepository(dbHelper);
            var cuotaRepo = new CuotaRepository(dbHelper);

            _socioService = new SocioService(socioRepo);
            _cuotaService = new CuotaService(cuotaRepo, socioRepo);

            // Creación de los controles del formulario
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtDni = new TextBox();
            dtpFechaInscripcion = new DateTimePicker();
            dtpFechaVencimiento = new DateTimePicker();
            chkEstadoActivo = new CheckBox();
            cmbTipoSocio = new ComboBox();

            // Inicializa y posiciona los componentes en el formulario
            InitializeComponents();
        }

        /// <summary>
        /// Inicializa y configura todos los componentes visuales del formulario
        /// </summary>
        private void InitializeComponents()
        {
            // Variables para el posicionamiento de controles
            int top = 20;            // Posición vertical inicial
            int spacing = 35;         // Espaciado entre controles
            int labelWidth = 120;     // Ancho de las etiquetas
            int inputLeft = labelWidth + 20; // Posición horizontal de los campos de entrada
            int inputWidth = 250;     // Ancho de los campos de entrada

            // Configuración y posicionamiento de los TextBox
            txtNombre.SetBounds(inputLeft, top, inputWidth, 20);
            txtApellido.SetBounds(inputLeft, top + spacing, inputWidth, 20);
            txtDni.SetBounds(inputLeft, top + spacing * 2, inputWidth, 20);

            // Configuración de los DateTimePicker
            dtpFechaInscripcion.SetBounds(inputLeft, top + spacing * 3, inputWidth, 20);
            dtpFechaInscripcion.Format = DateTimePickerFormat.Short; // Formato corto de fecha

            dtpFechaVencimiento.SetBounds(inputLeft, top + spacing * 4, inputWidth, 20);
            dtpFechaVencimiento.Format = DateTimePickerFormat.Short;

            // Configuración del CheckBox
            chkEstadoActivo.SetBounds(inputLeft, top + spacing * 5, inputWidth, 20);
            chkEstadoActivo.Text = "Activo";          // Texto junto al checkbox
            chkEstadoActivo.Checked = true;           // Estado inicial: marcado

            // Configuración del ComboBox para tipos de socio
            cmbTipoSocio.SetBounds(inputLeft, top + spacing * 6, inputWidth, 21);
            cmbTipoSocio.DropDownStyle = ComboBoxStyle.DropDownList; // No permite escritura
            cmbTipoSocio.Items.AddRange(Enum.GetNames(typeof(TipoSocio))); // Llena con valores del enum
            cmbTipoSocio.SelectedIndex = 0;           // Selecciona el primer item por defecto

            // Agrega las etiquetas descriptivas para cada control
            this.Controls.Add(CreateLabel("Nombre:", top));
            this.Controls.Add(CreateLabel("Apellido:", top + spacing));
            this.Controls.Add(CreateLabel("DNI:", top + spacing * 2));
            this.Controls.Add(CreateLabel("Fecha Inscripción:", top + spacing * 3));
            this.Controls.Add(CreateLabel("Vencimiento:", top + spacing * 4));
            this.Controls.Add(CreateLabel("Tipo de Socio:", top + spacing * 6));

            // Creación y configuración de botones
            var btnGuardar = new Button { Text = "Guardar Socio" };
            var btnSalir = new Button { Text = "Salir" };
            var btnBuscarEliminar = new Button { Text = "Buscar/Eliminar Socio" };

            // Cálculo de posiciones para centrar los botones
            int buttonWidth = 120;
            int spacingBetweenButtons = 10;
            int totalButtonsWidth = (buttonWidth * 3) + (spacingBetweenButtons * 2);
            int leftPosition = (this.ClientSize.Width - totalButtonsWidth) / 2;

            // Posicionamiento y configuración de eventos de los botones
            btnGuardar.SetBounds(leftPosition, top + spacing * 7, buttonWidth, 30);
            btnGuardar.Click += btnGuardar_Click; // Asigna el manejador de eventos

            btnBuscarEliminar.SetBounds(leftPosition + buttonWidth + spacingBetweenButtons, top + spacing * 7, buttonWidth, 30);
            btnBuscarEliminar.Click += btnBuscarEliminar_Click;

            btnSalir.SetBounds(leftPosition + (buttonWidth + spacingBetweenButtons) * 2, top + spacing * 7, buttonWidth, 30);
            btnSalir.Click += (sender, e) => this.Close(); // Cierra el formulario al hacer click

            // Agrega todos los controles al formulario
            this.Controls.AddRange(new Control[] {
                txtNombre, txtApellido, txtDni,
                dtpFechaInscripcion, dtpFechaVencimiento,
                chkEstadoActivo, cmbTipoSocio,
                btnGuardar, btnBuscarEliminar, btnSalir
            });
        }

        /// <summary>
        /// Crea una etiqueta con configuración consistente
        /// </summary>
        /// <param name="text">Texto a mostrar en la etiqueta</param>
        /// <param name="top">Posición vertical de la etiqueta</param>
        /// <returns>Objeto Label configurado</returns>
        private Label CreateLabel(string text, int top)
        {
            return new Label
            {
                Text = text,
                Left = 20,                       // Posición horizontal fija
                Top = top,                       // Posición vertical recibida como parámetro
                Width = 120,                     // Ancho fijo para alineación
                TextAlign = ContentAlignment.MiddleRight // Alineación del texto a la derecha
            };
        }

        /// <summary>
        /// Manejador del evento Click para el botón Guardar
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validación de campos obligatorios
            if (!string.IsNullOrWhiteSpace(txtNombre.Text) &&
                !string.IsNullOrWhiteSpace(txtApellido.Text) &&
                !string.IsNullOrWhiteSpace(txtDni.Text))
            {
                /////// Verifica si el DNI ya está registrado//////
                if (_socioService.ExisteDni(txtDni.Text)) //LLAMADA EL METODO DE SOCIOSERVICE PASA EL TEXTO DEL DNI COMO PARAMETRO
                {
                    MessageBox.Show("El DNI ingresado ya está registrado. Por favor ingrese un DNI diferente.");
                    txtDni.Focus(); // Coloca el foco en el campo DNI
                    return;
                }

                // Obtiene el tipo de socio seleccionado
                var tipoNombre = cmbTipoSocio.SelectedItem.ToString();
                TipoSocio tipo;
                Enum.TryParse(tipoNombre, out tipo);

                // Crea un nuevo objeto Socio con los datos del formulario
                var nuevoSocio = new Socio
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Dni = txtDni.Text,
                    FechaInscripcion = dtpFechaInscripcion.Value,
                    FechaVencimientoCuota = dtpFechaVencimiento.Value,
                    EstadoActivo = chkEstadoActivo.Checked,
                    Tipo = tipo
                };

                // Registra el nuevo socio usando el servicio,llama al servicio
                _socioService.RegistrarSocio(nuevoSocio);
                MessageBox.Show("Socio registrado correctamente!");

                // Limpia el formulario para permitir un nuevo registro
                LimpiarFormulario();
            }
            else
            {
                // Muestra mensaje si faltan campos obligatorios
                MessageBox.Show("Por favor complete todos los campos obligatorios (Nombre, Apellido y DNI)");
            }
        }

        /// <summary>
        /// Manejador del evento Click para el botón Buscar/Eliminar
        /// </summary>
        private void btnBuscarEliminar_Click(object sender, EventArgs e)
        {
            // Crea y muestra el formulario de búsqueda/eliminación
            var buscarEliminarForm = new BuscarEliminarSocioForm(_socioService);
            buscarEliminarForm.ShowDialog(); // Muestra como diálogo modal
        }

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDni.Clear();
            dtpFechaInscripcion.Value = DateTime.Today; // Fecha actual
            dtpFechaVencimiento.Value = DateTime.Today.AddMonths(1); // Un mes después
            chkEstadoActivo.Checked = true; // Estado activo por defecto
            cmbTipoSocio.SelectedIndex = 0; // Primer tipo de socio
        }
    }
}