
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
        private readonly ICuotaService _cuotaService;
        private readonly ICuotaRepository _cuotaRepository;

        private TextBox txtDni;
        private Button btnBuscar;
        private Label lblNombre;
        private Label lblApellido;
        private Label lblEstado;
        private Label lblVencimiento;
        private Label lblMonto;
        private ComboBox cmbMetodoPago;
        private Button btnPagar;
        private Button btnCancelar;

        public PagoCuotaForm(ICuotaService cuotaService, ICuotaRepository cuotaRepository)
        {
            _cuotaService = cuotaService;
            _cuotaRepository = cuotaRepository;

            InitializeComponents();
            ConfigureForm();
        }

        private void InitializeComponents()
        {
            this.Text = "Pago de Cuota Social";
            this.Width = 400;
            this.Height = 350;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblDni = new Label { Text = "DNI del Socio:", Left = 20, Top = 20, Width = 100 };
            txtDni = new TextBox { Left = 130, Top = 20, Width = 150 };
            btnBuscar = new Button { Text = "Buscar", Left = 290, Top = 20, Width = 80 };

            lblNombre = new Label { Left = 20, Top = 60, Width = 350 };
            lblApellido = new Label { Left = 20, Top = 90, Width = 350 };
            lblEstado = new Label { Left = 20, Top = 120, Width = 350 };
            lblVencimiento = new Label { Left = 20, Top = 150, Width = 350 };

            decimal monto = _cuotaService.ObtenerValorCuota();
            lblMonto = new Label
            {
                Text = string.Format("Monto de la cuota: {0:C}", monto),
                Left = 20,
                Top = 180,
                Width = 350
            };

            Label lblMetodo = new Label { Text = "Método de Pago:", Left = 20, Top = 210, Width = 100 };
            cmbMetodoPago = new ComboBox { Left = 130, Top = 210, Width = 150 };
            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;

            foreach (MetodoPago metodo in Enum.GetValues(typeof(MetodoPago)))
            {
                cmbMetodoPago.Items.Add(metodo);
            }
            cmbMetodoPago.SelectedIndex = 0;

            btnPagar = new Button { Text = "Registrar Pago", Left = 100, Top = 270, Width = 100, Enabled = false };
            btnCancelar = new Button { Text = "Cancelar", Left = 220, Top = 270, Width = 100 };

            btnBuscar.Click += BtnBuscar_Click;
            btnPagar.Click += BtnPagar_Click;
            btnCancelar.Click += delegate { this.Close(); };

            this.Controls.AddRange(new Control[] {
                lblDni, txtDni, btnBuscar,
                lblNombre, lblApellido, lblEstado, lblVencimiento, lblMonto,
                lblMetodo, cmbMetodoPago,
                btnPagar, btnCancelar
            });
        }

        private void ConfigureForm()
        {
            this.AcceptButton = btnBuscar;
            this.CancelButton = btnCancelar;
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDni.Text))
                {
                    MessageBox.Show("Por favor ingrese un DNI válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var socio = _cuotaService.BuscarSocio(txtDni.Text.Trim());
                if (socio == null)
                {
                    MessageBox.Show("No se encontró un socio activo con ese DNI", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetearCampos();
                    return;
                }

                lblNombre.Text = string.Format("Nombre: {0}", socio.Nombre);
                lblApellido.Text = string.Format("Apellido: {0}", socio.Apellido);
                lblEstado.Text = string.Format("Estado: {0}", socio.EstadoActivo ? "ACTIVO" : "INACTIVO");

                DateTime fechaVencimiento = _cuotaRepository.ObtenerFechaVencimientoActual(socio.Id);
                lblVencimiento.Text = string.Format("Próximo vencimiento: {0:dd/MM/yyyy}", fechaVencimiento);

                btnPagar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al buscar socio: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetearCampos();
            }
        }

        

        private void BtnPagar_Click(object sender, EventArgs e)
        {
            try
            {
                var socio = _cuotaService.BuscarSocio(txtDni.Text.Trim());
                if (socio == null)
                {
                    MessageBox.Show("No se encontró el socio. Por favor busque nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MetodoPago metodoPago = (MetodoPago)cmbMetodoPago.SelectedItem;
                decimal monto = _cuotaService.ObtenerValorCuota();

                DialogResult confirmacion = MessageBox.Show(
                    string.Format("¿Confirmar pago de {0:C} por {1} {2}?", monto, socio.Nombre, socio.Apellido),
                    "Confirmar Pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    _cuotaService.ProcesarPago(socio.Id, monto, metodoPago);
                    MessageBox.Show("Pago registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetearCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al procesar el pago: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetearCampos()
        {
            lblNombre.Text = string.Empty;
            lblApellido.Text = string.Empty;
            lblEstado.Text = string.Empty;
            lblVencimiento.Text = string.Empty;
            btnPagar.Enabled = false;
            txtDni.Focus();
        }
    }
}