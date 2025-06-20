using System;
using System.Windows.Forms;
using ClubDeportivo.Services;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;

namespace ClubDeportivo.Views.Forms
{
    public partial class frmGestionCarnet : Form
    {
        private readonly ISocioService _socioService;
        private readonly ICarnetService _carnetService;
        private Socio _socioSeleccionado;

        public frmGestionCarnet(ISocioService socioService, ICarnetService carnetService)
        {
            InitializeComponent();
            _socioService = socioService;
            _carnetService = carnetService;
            ConfigurarControles();
        }

        private void ConfigurarControles()
        {
            this.Text = "Gestión de Carnet de Socio";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            txtDniSocio.Clear();
            btnBuscarPorDni.Click += btnBuscarPorDni_Click;
            HabilitarControlesCarnet(false);

            // Limpiar
            btnCancelar.Text = "Limpiar";
            
            // Salir 
           var btnSalir = new Button
            {
                Text = "Salir",
                Size = btnCancelar.Size
            };

            // centrar los botones
            int espacioEntreBotones = 10;
            int anchoTotalBotones = btnCancelar.Width + btnSalir.Width + espacioEntreBotones;
            int posicionX = (this.ClientSize.Width - anchoTotalBotones) / 2;

            // Posicionar botones
            btnCancelar.Location = new System.Drawing.Point(posicionX, btnCancelar.Location.Y);
            btnSalir.Location = new System.Drawing.Point(posicionX + btnCancelar.Width + espacioEntreBotones, btnCancelar.Location.Y);

            btnSalir.Click += (sender, e) => this.Close();
            this.Controls.Add(btnSalir);
        }

       
        private void HabilitarControlesCarnet(bool habilitar)
        {
            chkAptoFisico.Enabled = habilitar;
            btnGenerar.Enabled = habilitar;
            btnConfirmarEntrega.Enabled = false;
        }

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

                _socioSeleccionado = _socioService.GetSocio(txtDniSocio.Text.Trim());

                if (_socioSeleccionado == null)
                {
                    MessageBox.Show("No se encontró un socio con el DNI ingresado", "Información",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarControles();
                    return;
                }

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

        private void MostrarDatosSocio()
        {
            lblNombre.Text = _socioSeleccionado.Nombre;
            lblApellido.Text = _socioSeleccionado.Apellido;
            lblDniValue.Text = _socioSeleccionado.Dni;
        }

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

        private void LimpiarControles()
        {
            lblNombre.Text = string.Empty;
            lblApellido.Text = string.Empty;
            lblDniValue.Text = string.Empty;
            LimpiarDatosCarnet();
            _socioSeleccionado = null;
            HabilitarControlesCarnet(false);
        }

        private void LimpiarDatosCarnet()
        {
            lblNroCarnet.Text = string.Empty;
            lblFechaEmision.Text = string.Empty;
            lblFechaVencimiento.Text = string.Empty;
            chkAptoFisico.Checked = false;
        }

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

                _carnetService.GenerateCarnetForSocio(_socioSeleccionado.Id, chkAptoFisico.Checked);
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            txtDniSocio.Clear();
            txtDniSocio.Focus();
        }
    }
}