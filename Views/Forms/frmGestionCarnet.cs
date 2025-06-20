using System;
using System.Windows.Forms;
using ClubDeportivo.Services;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;

namespace ClubDeportivo.Views.Forms
{
    public partial class frmGestionCarnet : Form
    {
        // Servicios para manejar socios y carnets
        private readonly ISocioService _socioService;
        private readonly ICarnetService _carnetService;

        // Socio actualmente seleccionado en la interfaz
        private Socio _socioSeleccionado;

        // Constructor que recibe los servicios por inyección
        public frmGestionCarnet(ISocioService socioService, ICarnetService carnetService)
        {
            InitializeComponent();
            _socioService = socioService;
            _carnetService = carnetService;
            ConfigurarControles();
        }

        // Configura el formulario y sus controles iniciales
        private void ConfigurarControles()
        {
            this.Text = "Gestión de Carnet de Socio";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            txtDniSocio.Clear();
            btnBuscarPorDni.Click += btnBuscarPorDni_Click;

            HabilitarControlesCarnet(false);

            // Cambia el texto del botón cancelar a "Limpiar"
            btnCancelar.Text = "Limpiar";

            // Crear botón Salir
            var btnSalir = new Button
            {
                Text = "Salir",
                Size = btnCancelar.Size
            };

            // Calcular posición para centrar los botones Limpiar y Salir
            int espacioEntreBotones = 10;
            int anchoTotalBotones = btnCancelar.Width + btnSalir.Width + espacioEntreBotones;
            int posicionX = (this.ClientSize.Width - anchoTotalBotones) / 2;

            // Posicionar los botones en la ventana
            btnCancelar.Location = new System.Drawing.Point(posicionX, btnCancelar.Location.Y);
            btnSalir.Location = new System.Drawing.Point(posicionX + btnCancelar.Width + espacioEntreBotones, btnCancelar.Location.Y);

            // Evento para cerrar el formulario al hacer click en salir
            btnSalir.Click += (sender, e) => this.Close();

            // Añade el botón Salir al formulario
            this.Controls.Add(btnSalir);
        }

        // Habilita o deshabilita controles relacionados al carnet
        private void HabilitarControlesCarnet(bool habilitar)
        {
            chkAptoFisico.Enabled = habilitar;
            btnGenerar.Enabled = habilitar;
            btnConfirmarEntrega.Enabled = false;
        }

        // Evento click para buscar un socio por DNI
        private void btnBuscarPorDni_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDniSocio.Text))
                {
                    MessageBox.Show("Por favor ingrese el DNI del socio", "Advertencia",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Buscar el socio usando el servicio
                _socioSeleccionado = _socioService.GetSocio(txtDniSocio.Text.Trim());

                if (_socioSeleccionado == null)
                {
                    MessageBox.Show("No se encontró un socio con el DNI ingresado", "Información",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarControles();
                    return;
                }

                // Mostrar datos del socio y verificar si ya tiene carnet
                MostrarDatosSocio();
                VerificarCarnetExistente();
                HabilitarControlesCarnet(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar socio: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Muestra los datos del socio en la interfaz
        private void MostrarDatosSocio()
        {
            lblNombre.Text = _socioSeleccionado.Nombre;
            lblApellido.Text = _socioSeleccionado.Apellido;
            lblDniValue.Text = _socioSeleccionado.Dni;
        }

        // Verifica si el socio ya tiene un carnet y muestra sus datos si existe
        private void VerificarCarnetExistente()
        {
            var carnet = _carnetService.GetCarnetBySocio(_socioSeleccionado.Id);

            if (carnet != null)
            {
                lblNroCarnet.Text = carnet.NroCarnet.ToString();
                lblFechaEmision.Text = carnet.FechaEmision.ToShortDateString();
                lblFechaVencimiento.Text = carnet.FechaVencimiento.ToShortDateString();
                chkAptoFisico.Checked = carnet.AptoFisico;
                btnGenerar.Enabled = false;
                btnConfirmarEntrega.Enabled = true;
            }
            else
            {
                LimpiarDatosCarnet();
                btnGenerar.Enabled = true;
                btnConfirmarEntrega.Enabled = false;
            }
        }

        // Limpia los controles relacionados con la info del socio y carnet
        private void LimpiarControles()
        {
            lblNombre.Text = string.Empty;
            lblApellido.Text = string.Empty;
            lblDniValue.Text = string.Empty;
            LimpiarDatosCarnet();
            _socioSeleccionado = null;
            HabilitarControlesCarnet(false);
        }

        // Limpia los datos del carnet en pantalla
        private void LimpiarDatosCarnet()
        {
            lblNroCarnet.Text = string.Empty;
            lblFechaEmision.Text = string.Empty;
            lblFechaVencimiento.Text = string.Empty;
            chkAptoFisico.Checked = false;
        }

        // Evento click para generar carnet al socio
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (_socioSeleccionado == null)
            {
                MessageBox.Show("Primero debe buscar un socio válido.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!chkAptoFisico.Checked)
                {
                    MessageBox.Show("Debe verificar que el socio está apto físicamente.", "Advertencia",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Generar carnet a través del servicio
                _carnetService.GenerateCarnetForSocio(_socioSeleccionado.Id, chkAptoFisico.Checked);

                // Actualizar datos del carnet en la UI
                VerificarCarnetExistente();

                MessageBox.Show("Carnet generado exitosamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar carnet: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento click para confirmar la entrega del carnet
        private void btnConfirmarEntrega_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Entrega de carnet confirmada.", "Confirmación",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarControles();
                txtDniSocio.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar entrega: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento click para limpiar el formulario
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            txtDniSocio.Clear();
            txtDniSocio.Focus();
        }
    }
}
