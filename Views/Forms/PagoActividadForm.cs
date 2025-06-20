using System;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Services;
using ClubDeportivo.Interfaces;

namespace ClubDeportivo.Views.Forms
{
    public class PagoActividadForm : Form
    {
        private readonly INoSocioService _noSocioService;
        private readonly IActividadService _actividadService;
        private readonly IPagoService _pagoService;
        
        private ComboBox cmbActividades;
        private ComboBox cmbMetodoPago;
        private TextBox txtDniNoSocio;
        private Label lblInfoNoSocio;
        private Label lblPrecio;
        private Button btnBuscar;
        private Button btnPagar;

        public PagoActividadForm(
            INoSocioService noSocioService,
            IActividadService actividadService,
            IPagoService pagoService)
        {
            _noSocioService = noSocioService;
            _actividadService = actividadService;
            _pagoService = pagoService;
            
            InitializeComponent();
            CargarActividades();
            CargarMetodosPago();
        }

        private void InitializeComponent()
        {
            this.Text = "Pago de Actividades";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Controles para No Socio
            var lblDni = new Label { Text = "DNI No Socio:", Left = 20, Top = 20 };
            txtDniNoSocio = new TextBox { Left = 120, Top = 20, Width = 150 };
            btnBuscar = new Button { Text = "Buscar", Left = 280, Top = 20, Width = 80 };
            lblInfoNoSocio = new Label { Left = 120, Top = 50, Width = 300 };

            // Controles para Actividad
            var lblActividad = new Label { Text = "Actividad:", Left = 20, Top = 90 };
            cmbActividades = new ComboBox { Left = 120, Top = 90, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

            // Controles para Método de Pago
            var lblMetodo = new Label { Text = "Método Pago:", Left = 20, Top = 130 };
            cmbMetodoPago = new ComboBox { Left = 120, Top = 130, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            // Precio
            var lblPrecioText = new Label { Text = "Precio:", Left = 20, Top = 170 };
            lblPrecio = new Label { Left = 120, Top = 170, Width = 100 };

            // Botón Pagar
            btnPagar = new Button { Text = "Registrar Pago", Left = 180, Top = 220, Width = 150, Enabled = false };

            // Eventos
            btnBuscar.Click += BtnBuscar_Click;
            cmbActividades.SelectedIndexChanged += CmbActividades_SelectedIndexChanged;
            btnPagar.Click += BtnPagar_Click;

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
            cmbActividades.DataSource = _actividadService.ObtenerActividadesParaNoSocios();
            cmbActividades.DisplayMember = "Nombre";
            cmbActividades.ValueMember = "Id";
        }

        private void CargarMetodosPago()
        {
            cmbMetodoPago.DataSource = Enum.GetValues(typeof(MetodoPago));
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDniNoSocio.Text))
            {
                MessageBox.Show("Ingrese un DNI válido");
                return;
            }

            var noSocio = _noSocioService.BuscarPorDni(txtDniNoSocio.Text);
            if (noSocio == null)
            {
                MessageBox.Show("No socio no encontrado");
                return;
            }

            lblInfoNoSocio.Text = string.Format("No Socio: {0} {1}", noSocio.Nombre, noSocio.Apellido);
            btnPagar.Enabled = true;
        }


        private void CmbActividades_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                var actividad = (Actividad)cmbActividades.SelectedItem;
                var metodo = (MetodoPago)cmbMetodoPago.SelectedItem;

                int noSocioId = ObtenerIdNoSocio(txtDniNoSocio.Text);

                _pagoService.ProcesarPago(
                    noSocioId,
                    actividad.Id,
                    actividad.Precio,
                    metodo);

                MessageBox.Show("Pago registrado exitosamente!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al procesar pago: {0}", ex.Message));
            }
        }

        private int ObtenerIdNoSocio(string dni)
        {
            var noSocio = _noSocioService.BuscarPorDni(dni);
            if (noSocio == null)
            {
                throw new Exception("No Socio no encontrado");
            }
            return noSocio.Id;
        }
    }
}