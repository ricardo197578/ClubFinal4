using System;
using System.Drawing;
using System.Windows.Forms;
using ClubDeportivo.Services;
using ClubDeportivo.Interfaces;

namespace ClubDeportivo.Views.Forms
{
    public class LoginForm : Form
    {
        private readonly IAuthService _authService;  // Servicio para autenticación
        private TextBox txtUsuario;                   // Campo para el nombre de usuario
        private TextBox txtPassword;                  // Campo para la contraseña
        private Button btnLogin;                      // Botón para iniciar sesión
        private Button btnCancelar;                   // Botón para cancelar / cerrar formulario
        private Label lblUsuario;                     // Etiqueta para usuario
        private Label lblPassword;                    // Etiqueta para contraseña
        private PictureBox logoPictureBox;            // Imagen/logo en el formulario

        public LoginForm(IAuthService authService)
        {
            _authService = authService;
            InitializeControls();   // Crear y configurar controles visuales
            SetupForm();            // Configurar comportamiento general del formulario
        }

        private void InitializeControls()
        {
            // Configuración básica del formulario
            this.Text = "Inicio de Sesión - Club Deportivo";
            this.ClientSize = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar al abrir
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // Tamaño fijo, sin redimensionar
            this.MaximizeBox = false;  // Desactivar maximizar
            this.MinimizeBox = false;  // Desactivar minimizar

            // PictureBox para mostrar un logo o icono (aquí usa un icono estándar)
            logoPictureBox = new PictureBox
            {
                Image = SystemIcons.Information.ToBitmap(), // Reemplazar por logo propio
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(100, 100),
                Location = new Point(125, 20)
            };

            // Label que indica "Usuario:"
            lblUsuario = new Label
            {
                Text = "Usuario:",
                Location = new Point(50, 140),
                Size = new Size(100, 20)
            };

            // TextBox para que el usuario ingrese su nombre de usuario
            txtUsuario = new TextBox
            {
                Location = new Point(150, 140),
                Size = new Size(150, 20),
                MaxLength = 20  // Limitar longitud máxima a 20 caracteres
            };

            // Label que indica "Contraseña:"
            lblPassword = new Label
            {
                Text = "Contraseña:",
                Location = new Point(50, 170),
                Size = new Size(100, 20)
            };

            // TextBox para ingresar la contraseña, con carácter oculto
            txtPassword = new TextBox
            {
                Location = new Point(150, 170),
                Size = new Size(150, 20),
                PasswordChar = '*',  // Ocultar caracteres ingresados
                MaxLength = 20
            };

            // Botón para iniciar sesión
            btnLogin = new Button
            {
                Text = "Iniciar Sesión",
                Location = new Point(70, 210),
                Size = new Size(100, 30)
            };
            btnLogin.Click += btnLogin_Click;  // Evento click para validar login

            // Botón para cancelar o cerrar el formulario
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(180, 210),
                Size = new Size(100, 30)
            };
            btnCancelar.Click += btnCancelar_Click; // Evento click para cerrar formulario

            // Agregar todos los controles al formulario
            this.Controls.Add(logoPictureBox);
            this.Controls.Add(lblUsuario);
            this.Controls.Add(txtUsuario);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnCancelar);
        }

        private void SetupForm()
        {
            this.AcceptButton = btnLogin;   // Enter activará btnLogin
            this.CancelButton = btnCancelar; // Esc activará btnCancelar
        }

        // Evento click del botón iniciar sesión
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validar que usuario y contraseña no estén vacíos o con solo espacios
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Usuario y contraseña son requeridos", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Salir sin continuar
            }

            // Intentar autenticar con el servicio
            if (_authService.Autenticar(txtUsuario.Text, txtPassword.Text))
            {
                this.DialogResult = DialogResult.OK; // Login exitoso
                this.Close();                        // Cerrar formulario
            }
            else
            {
                // Credenciales inválidas: mostrar error y limpiar contraseña
                MessageBox.Show("Credenciales inválidas", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUsuario.Focus();  // Enfocar en usuario para reintentar
            }
        }

        // Evento click del botón cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Cancelar operación
            this.Close();                            // Cerrar formulario
        }
    }
}
