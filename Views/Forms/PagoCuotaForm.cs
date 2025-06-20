
using System;
using System.Windows.Forms;
using ClubDeportivo.Services;
using ClubDeportivo.Repositories;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;

namespace ClubDeportivo.Views.Forms
{
    public class PagoCuotaForm : Form
    {
        private readonly ICuotaService _cuotaService;        // Servicio para manejar cuotas
        private readonly ICuotaRepository _cuotaRepository;  // Repositorio para operaciones de cuota

        private TextBox txtDni;               // TextBox para ingresar DNI del socio
        private Button btnBuscar;             // Botón para buscar socio por DNI
        private Label lblNombre;              // Label para mostrar nombre del socio
        private Label lblApellido;            // Label para mostrar apellido del socio
        private Label lblEstado;              // Label para mostrar estado del socio (activo/inactivo)
        private Label lblVencimiento;         // Label para mostrar próxima fecha de vencimiento
        private Label lblMonto;               // Label para mostrar monto de la cuota
        private ComboBox cmbMetodoPago;       // ComboBox para seleccionar método de pago
        private Button btnPagar;              // Botón para registrar el pago
        private Button btnCancelar;           // Botón para cancelar y cerrar formulario

        public PagoCuotaForm(ICuotaService cuotaService, ICuotaRepository cuotaRepository)
        {
            _cuotaService = cuotaService;          // Inyección de dependencia del servicio cuota
            _cuotaRepository = cuotaRepository;    // Inyección de dependencia del repositorio cuota

            InitializeComponents();                 // Inicializar controles UI
            ConfigureForm();                       // Configurar propiedades del formulario
        }

        private void InitializeComponents()
        {
            this.Text = "Pago de Cuota Social";    // Título del formulario
            this.Width = 400;                       // Ancho del formulario
            this.Height = 350;                      // Alto del formulario
            this.StartPosition = FormStartPosition.CenterScreen;  // Centrar pantalla
            this.FormBorderStyle = FormBorderStyle.FixedDialog;   // No redimensionable
            this.MaximizeBox = false;               // Sin botón maximizar

            // Etiqueta DNI
            Label lblDni = new Label { Text = "DNI del Socio:", Left = 20, Top = 20, Width = 100 };
            // TextBox para ingresar DNI
            txtDni = new TextBox { Left = 130, Top = 20, Width = 150 };
            // Botón Buscar socio
            btnBuscar = new Button { Text = "Buscar", Left = 290, Top = 20, Width = 80 };

            // Labels para mostrar información del socio
            lblNombre = new Label { Left = 20, Top = 60, Width = 350 };
            lblApellido = new Label { Left = 20, Top = 90, Width = 350 };
            lblEstado = new Label { Left = 20, Top = 120, Width = 350 };
            lblVencimiento = new Label { Left = 20, Top = 150, Width = 350 };

            // Obtener monto cuota desde el servicio
            decimal monto = _cuotaService.ObtenerValorCuota();
            lblMonto = new Label
            {
                Text = string.Format("Monto de la cuota: {0:C}", monto),
                Left = 20,
                Top = 180,
                Width = 350
            };

            // Etiqueta método pago
            Label lblMetodo = new Label { Text = "Método de Pago:", Left = 20, Top = 210, Width = 100 };
            // ComboBox método pago
            cmbMetodoPago = new ComboBox { Left = 130, Top = 210, Width = 150 };
            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;

            // Cargar métodos de pago en ComboBox
            foreach (MetodoPago metodo in Enum.GetValues(typeof(MetodoPago)))
            {
                cmbMetodoPago.Items.Add(metodo);
            }
            cmbMetodoPago.SelectedIndex = 0; // Seleccionar el primer método por defecto

            // Botones Pagar y Cancelar
            btnPagar = new Button { Text = "Registrar Pago", Left = 100, Top = 270, Width = 100, Enabled = false }; // Inhabilitado hasta buscar socio
            btnCancelar = new Button { Text = "Cancelar", Left = 220, Top = 270, Width = 100 };

            // Asociar eventos
            btnBuscar.Click += BtnBuscar_Click;
            btnPagar.Click += BtnPagar_Click;
            btnCancelar.Click += delegate { this.Close(); };

            // Agregar controles al formulario
            this.Controls.AddRange(new Control[] {
                lblDni, txtDni, btnBuscar,
                lblNombre, lblApellido, lblEstado, lblVencimiento, lblMonto,
                lblMetodo, cmbMetodoPago,
                btnPagar, btnCancelar
            });
        }

        private void ConfigureForm()
        {
            this.AcceptButton = btnBuscar;    // Enter activa botón Buscar
            this.CancelButton = btnCancelar;  // Esc activa botón Cancelar
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que se ingresó un DNI válido
                if (string.IsNullOrWhiteSpace(txtDni.Text))
                {
                    MessageBox.Show("Por favor ingrese un DNI válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Buscar socio activo por DNI usando servicio
                var socio = _cuotaService.BuscarSocio(txtDni.Text.Trim());
                if (socio == null)
                {
                    MessageBox.Show("No se encontró un socio activo con ese DNI", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetearCampos(); // Limpiar pantalla si no se encuentra
                    return;
                }

                // Mostrar datos del socio encontrado
                lblNombre.Text = string.Format("Nombre: {0}", socio.Nombre);
                lblApellido.Text = string.Format("Apellido: {0}", socio.Apellido);
                lblEstado.Text = string.Format("Estado: {0}", socio.EstadoActivo ? "ACTIVO" : "INACTIVO");

                // Obtener la próxima fecha de vencimiento de la cuota
                DateTime fechaVencimiento = _cuotaRepository.ObtenerFechaVencimientoActual(socio.Id);
                lblVencimiento.Text = string.Format("Próximo vencimiento: {0:dd/MM/yyyy}", fechaVencimiento);

                btnPagar.Enabled = true; // Habilitar botón para registrar pago
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al buscar socio: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetearCampos(); // Limpiar en caso de error
            }
        }



        private void BtnPagar_Click(object sender, EventArgs e)
        {
            try
            {
                // Buscar socio nuevamente para asegurar datos actuales
                var socio = _cuotaService.BuscarSocio(txtDni.Text.Trim());
                if (socio == null)
                {
                    MessageBox.Show("No se encontró el socio. Por favor busque nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MetodoPago metodoPago = (MetodoPago)cmbMetodoPago.SelectedItem;    // Obtener método pago seleccionado
                decimal monto = _cuotaService.ObtenerValorCuota();                 // Obtener monto cuota

                // Confirmar pago con mensaje al usuario
                DialogResult confirmacion = MessageBox.Show(
                    string.Format("¿Confirmar pago de {0:C} por {1} {2}?", monto, socio.Nombre, socio.Apellido),
                    "Confirmar Pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    // Procesar pago con el servicio
                    _cuotaService.ProcesarPago(socio.Id, monto, metodoPago);
                    MessageBox.Show("Pago registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetearCampos(); // Limpiar campos para siguiente operación
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al procesar el pago: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetearCampos()
        {
            // Limpiar textos y deshabilitar botón pagar
            lblNombre.Text = string.Empty;
            lblApellido.Text = string.Empty;
            lblEstado.Text = string.Empty;
            lblVencimiento.Text = string.Empty;
            btnPagar.Enabled = false;
            txtDni.Focus(); // Poner foco en textbox DNI para nuevo ingreso
        }
    }
}
