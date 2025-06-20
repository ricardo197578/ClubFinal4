using System;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Services;
using ClubDeportivo.Interfaces;

namespace ClubDeportivo.Views.Forms
{
    public class PagoActividadForm : Form
    {
        private readonly INoSocioService _noSocioService;     // Servicio para manejar No Socios
        private readonly IActividadService _actividadService; // Servicio para manejar Actividades
        private readonly IPagoService _pagoService;           // Servicio para manejar Pagos

        private ComboBox cmbActividades;   // ComboBox para seleccionar la actividad
        private ComboBox cmbMetodoPago;    // ComboBox para seleccionar método de pago
        private TextBox txtDniNoSocio;     // TextBox para ingresar DNI del No Socio
        private Label lblInfoNoSocio;      // Label para mostrar info del No Socio encontrado
        private Label lblPrecio;           // Label para mostrar el precio de la actividad seleccionada
        private Button btnBuscar;          // Botón para buscar No Socio por DNI
        private Button btnPagar;           // Botón para registrar pago

        public PagoActividadForm(
            INoSocioService noSocioService,
            IActividadService actividadService,
            IPagoService pagoService)
        {
            _noSocioService = noSocioService;       // Inyectar servicio No Socio
            _actividadService = actividadService;   // Inyectar servicio Actividad
            _pagoService = pagoService;             // Inyectar servicio Pago

            InitializeComponent();                   // Inicializar controles y UI
            CargarActividades();                     // Cargar actividades al ComboBox
            CargarMetodosPago();                     // Cargar métodos de pago al ComboBox
        }

        private void InitializeComponent()
        {
            this.Text = "Pago de Actividades";       // Título del formulario
            this.Width = 500;                         // Ancho del formulario
            this.Height = 400;                        // Alto del formulario
            this.StartPosition = FormStartPosition.CenterScreen;  // Centrar formulario

            // Controles para No Socio
            var lblDni = new Label { Text = "DNI No Socio:", Left = 20, Top = 20 };  // Etiqueta DNI
            txtDniNoSocio = new TextBox { Left = 120, Top = 20, Width = 150 };       // Input DNI
            btnBuscar = new Button { Text = "Buscar", Left = 280, Top = 20, Width = 80 };  // Botón buscar
            lblInfoNoSocio = new Label { Left = 120, Top = 50, Width = 300 };        // Label info No Socio

            // Controles para Actividad
            var lblActividad = new Label { Text = "Actividad:", Left = 20, Top = 90 };  // Etiqueta actividad
            cmbActividades = new ComboBox { Left = 120, Top = 90, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };  // ComboBox actividades

            // Controles para Método de Pago
            var lblMetodo = new Label { Text = "Método Pago:", Left = 20, Top = 130 }; // Etiqueta método pago
            cmbMetodoPago = new ComboBox { Left = 120, Top = 130, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList }; // ComboBox métodos pago

            // Precio
            var lblPrecioText = new Label { Text = "Precio:", Left = 20, Top = 170 };  // Etiqueta precio
            lblPrecio = new Label { Left = 120, Top = 170, Width = 100 };              // Label que muestra precio

            // Botón Pagar
            btnPagar = new Button { Text = "Registrar Pago", Left = 180, Top = 220, Width = 150, Enabled = false };  // Botón para registrar pago, inicialmente deshabilitado

            // Eventos asociados a controles
            btnBuscar.Click += BtnBuscar_Click;                       // Al hacer click en buscar
            cmbActividades.SelectedIndexChanged += CmbActividades_SelectedIndexChanged;  // Cambiar selección actividad
            btnPagar.Click += BtnPagar_Click;                         // Al hacer click en pagar

            // Agregar todos los controles al formulario
            this.Controls.AddRange(new Control[] {
                lblDni, txtDniNoSocio, btnBuscar, lblInfoNoSocio,
                lblActividad, cmbActividades,
                lblMetodo, cmbMetodoPago,
                lblPrecioText, lblPrecio,
                btnPagar
            });
        }

        private void CargarActividades()
        {
            // Cargar actividades disponibles para No Socios desde el servicio
            cmbActividades.DataSource = _actividadService.ObtenerActividadesParaNoSocios();
            cmbActividades.DisplayMember = "Nombre";  // Mostrar nombre en el combo
            cmbActividades.ValueMember = "Id";        // Valor del combo será el Id
        }

        private void CargarMetodosPago()
        {
            // Cargar métodos de pago desde el enum MetodoPago
            cmbMetodoPago.DataSource = Enum.GetValues(typeof(MetodoPago));
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            // Validar que el DNI no esté vacío o solo espacios
            if (string.IsNullOrWhiteSpace(txtDniNoSocio.Text))
            {
                MessageBox.Show("Ingrese un DNI válido");
                return;
            }

            // Buscar No Socio por DNI usando el servicio
            var noSocio = _noSocioService.BuscarPorDni(txtDniNoSocio.Text);
            if (noSocio == null)
            {
                MessageBox.Show("No socio no encontrado");
                return;
            }

            // Mostrar información del No Socio encontrado y habilitar botón pagar
            lblInfoNoSocio.Text = string.Format("No Socio: {0} {1}", noSocio.Nombre, noSocio.Apellido);
            btnPagar.Enabled = true;
        }

        private void CmbActividades_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cuando se cambia la selección en actividades, actualizar el label de precio
            var actividad = cmbActividades.SelectedItem as Actividad;
            if (actividad != null)
            {
                lblPrecio.Text = string.Format("${0}", actividad.Precio);
            }
        }

        private void BtnPagar_Click(object sender, EventArgs e)
        {
            try
            {
                var actividad = (Actividad)cmbActividades.SelectedItem;    // Obtener actividad seleccionada
                var metodo = (MetodoPago)cmbMetodoPago.SelectedItem;        // Obtener método de pago seleccionado

                int noSocioId = ObtenerIdNoSocio(txtDniNoSocio.Text);       // Obtener Id del No Socio

                // Procesar pago usando el servicio de pagos
                _pagoService.ProcesarPago(
                    noSocioId,
                    actividad.Id,
                    actividad.Precio,
                    metodo);

                MessageBox.Show("Pago registrado exitosamente!");           // Confirmación éxito
                this.Close();                                                // Cerrar formulario
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al procesar pago: {0}", ex.Message));  // Mostrar error si falla
            }
        }

        private int ObtenerIdNoSocio(string dni)
        {
            // Buscar No Socio por DNI, lanzar excepción si no existe
            var noSocio = _noSocioService.BuscarPorDni(dni);
            if (noSocio == null)
            {
                throw new Exception("No Socio no encontrado");
            }
            return noSocio.Id;  // Retornar Id del No Socio encontrado
        }
    }
}
