using System;
using System.Drawing;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;
using ClubDeportivo.Services;

namespace ClubDeportivo.Views.Forms
{
    public class SocioForm : Form
    {
        private readonly SocioService _socioService;
        private readonly CuotaService _cuotaService;
        private readonly TextBox txtNombre;
        private readonly TextBox txtApellido;
        private readonly TextBox txtDni;
        private readonly DateTimePicker dtpFechaInscripcion;
        private readonly DateTimePicker dtpFechaVencimiento;
        private readonly CheckBox chkEstadoActivo;
        private readonly ComboBox cmbTipoSocio;

        public SocioForm()
        {
            this.Text = "Gestión de Socios";
            this.Width = 450; 
            this.Height = 450; 
            this.StartPosition = FormStartPosition.CenterScreen;

            var dbHelper = new DatabaseHelper();
            var socioRepo = new SocioRepository(dbHelper);
            var cuotaRepo = new CuotaRepository(dbHelper);

            _socioService = new SocioService(socioRepo);
            _cuotaService = new CuotaService(cuotaRepo, socioRepo);

            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtDni = new TextBox();
            dtpFechaInscripcion = new DateTimePicker();
            dtpFechaVencimiento = new DateTimePicker();
            chkEstadoActivo = new CheckBox();
            cmbTipoSocio = new ComboBox();

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            int top = 20;
            int spacing = 35;
            int labelWidth = 120; 
            int inputLeft = labelWidth + 20; 
            int inputWidth = 250;

            txtNombre.SetBounds(inputLeft, top, inputWidth, 20);
            txtApellido.SetBounds(inputLeft, top + spacing, inputWidth, 20);
            txtDni.SetBounds(inputLeft, top + spacing * 2, inputWidth, 20);

            dtpFechaInscripcion.SetBounds(inputLeft, top + spacing * 3, inputWidth, 20);
            dtpFechaInscripcion.Format = DateTimePickerFormat.Short;

            dtpFechaVencimiento.SetBounds(inputLeft, top + spacing * 4, inputWidth, 20);
            dtpFechaVencimiento.Format = DateTimePickerFormat.Short;

            chkEstadoActivo.SetBounds(inputLeft, top + spacing * 5, inputWidth, 20);
            chkEstadoActivo.Text = "Activo";
            chkEstadoActivo.Checked = true;

            cmbTipoSocio.SetBounds(inputLeft, top + spacing * 6, inputWidth, 21);
            cmbTipoSocio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoSocio.Items.AddRange(Enum.GetNames(typeof(TipoSocio)));
            cmbTipoSocio.SelectedIndex = 0;

            this.Controls.Add(CreateLabel("Nombre:", top));
            this.Controls.Add(CreateLabel("Apellido:", top + spacing));
            this.Controls.Add(CreateLabel("DNI:", top + spacing * 2));
            this.Controls.Add(CreateLabel("Fecha Inscripción:", top + spacing * 3));
            this.Controls.Add(CreateLabel("Vencimiento:", top + spacing * 4));
            this.Controls.Add(CreateLabel("Tipo de Socio:", top + spacing * 6));

            var btnGuardar = new Button { Text = "Guardar Socio" };
            var btnSalir = new Button { Text = "Salir" };
            var btnBuscarEliminar = new Button { Text = "Buscar/Eliminar Socio" }; // Nuevo botón por las dudas<----

            // Centrar los botones para 2 botones
            /*int buttonWidth = 120;
            int totalButtonsWidth = buttonWidth * 2 + 20; // 20 es el espacio entre botones
            int leftPosition = (this.ClientSize.Width - totalButtonsWidth) / 2;

            btnGuardar.SetBounds(leftPosition, top + spacing * 7, buttonWidth, 30);
            btnGuardar.Click += btnGuardar_Click;

            btnSalir.SetBounds(leftPosition + buttonWidth + 20, top + spacing * 7, buttonWidth, 30);
            btnSalir.Click += (sender, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                txtNombre, txtApellido, txtDni,
                dtpFechaInscripcion, dtpFechaVencimiento,
                chkEstadoActivo, cmbTipoSocio, 
                btnGuardar, btnSalir
            });*/

            // Centrar los botones (ajustado para 3 botones)
            int buttonWidth = 120;
            int spacingBetweenButtons = 10;
            int totalButtonsWidth = (buttonWidth * 3) + (spacingBetweenButtons * 2);
            int leftPosition = (this.ClientSize.Width - totalButtonsWidth) / 2;

            btnGuardar.SetBounds(leftPosition, top + spacing * 7, buttonWidth, 30);
            btnGuardar.Click += btnGuardar_Click;

            btnBuscarEliminar.SetBounds(leftPosition + buttonWidth + spacingBetweenButtons, top + spacing * 7, buttonWidth, 30);
            btnBuscarEliminar.Click += btnBuscarEliminar_Click;

            btnSalir.SetBounds(leftPosition + (buttonWidth + spacingBetweenButtons) * 2, top + spacing * 7, buttonWidth, 30);
            btnSalir.Click += (sender, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                txtNombre, txtApellido, txtDni,
                dtpFechaInscripcion, dtpFechaVencimiento,
                chkEstadoActivo, cmbTipoSocio,
                btnGuardar, btnBuscarEliminar, btnSalir // Agregar el nuevo botón <----
    });
        }

        private Label CreateLabel(string text, int top)
        {
            return new Label
            {
                Text = text,
                Left = 20,
                Top = top,
                Width = 120, 
                TextAlign = ContentAlignment.MiddleRight
            };
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text) &&
                !string.IsNullOrWhiteSpace(txtApellido.Text) &&
                !string.IsNullOrWhiteSpace(txtDni.Text))
            {
                if (_socioService.ExisteDni(txtDni.Text))
                {
                    MessageBox.Show("El DNI ingresado ya está registrado. Por favor ingrese un DNI diferente.");
                    txtDni.Focus();
                    return;
                }

                var tipoNombre = cmbTipoSocio.SelectedItem.ToString();
                TipoSocio tipo;
                Enum.TryParse(tipoNombre, out tipo);

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

                _socioService.RegistrarSocio(nuevoSocio);
                MessageBox.Show("Socio registrado correctamente!");
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios (Nombre, Apellido y DNI)");
            }
        }

        private void btnBuscarEliminar_Click(object sender, EventArgs e)//metodo <-----
        {
            var buscarEliminarForm = new BuscarEliminarSocioForm(_socioService);
            buscarEliminarForm.ShowDialog();
                       
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDni.Clear();
            dtpFechaInscripcion.Value = DateTime.Today;
            dtpFechaVencimiento.Value = DateTime.Today.AddMonths(1);
            chkEstadoActivo.Checked = true;
            cmbTipoSocio.SelectedIndex = 0;
        }
    }
}