// En Views/Forms/LoginForm.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using ClubDeportivo.Services;
using ClubDeportivo.Interfaces;

namespace ClubDeportivo.Views.Forms
{
    public class LoginForm : Form
    {
        private readonly IAuthService _authService;
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancelar;
        private Label lblUsuario;
        private Label lblPassword;
        private PictureBox logoPictureBox;

        public LoginForm(IAuthService authService)
        {
            _authService = authService;
            InitializeControls();
            SetupForm();
        }

        private void InitializeControls()
        {
            // Configuración del formulario
            this.Text = "Inicio de Sesión - Club Deportivo";
            this.ClientSize = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // PictureBox para el logo
            logoPictureBox = new PictureBox
            {
                Image = SystemIcons.Information.ToBitmap(), // Reemplaza con tu imagen
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(100, 100),
                Location = new Point(125, 20)
            };

            // Label Usuario
            lblUsuario = new Label
            {
                Text = "Usuario:",
                Location = new Point(50, 140),
                Size = new Size(100, 20)
            };

            // TextBox Usuario
            txtUsuario = new TextBox
            {
                Location = new Point(150, 140),
                Size = new Size(150, 20),
                MaxLength = 20
            };

            // Label Contraseña
            lblPassword = new Label
            {
                Text = "Contraseña:",
                Location = new Point(50, 170),
                Size = new Size(100, 20)
            };

            // TextBox Contraseña
            txtPassword = new TextBox
            {
                Location = new Point(150, 170),
                Size = new Size(150, 20),
                PasswordChar = '*',
                MaxLength = 20
            };

            // Botón Login
            btnLogin = new Button
            {
                Text = "Iniciar Sesión",
                Location = new Point(70, 210),
                Size = new Size(100, 30)
            };
            btnLogin.Click += btnLogin_Click;

            // Botón Cancelar
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(180, 210),
                Size = new Size(100, 30)
            };
            btnCancelar.Click += btnCancelar_Click;

            // Agregar controles al formulario
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
            this.AcceptButton = btnLogin;  // Enter activa btnLogin
            this.CancelButton = btnCancelar;  // Esc activa btnCancelar
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Usuario y contraseña son requeridos", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_authService.Autenticar(txtUsuario.Text, txtPassword.Text))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales inválidas", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUsuario.Focus();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}