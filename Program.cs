using System;
using System.Windows.Forms;
using ClubDeportivo.Repositories;
using ClubDeportivo.Services;
using ClubDeportivo.Views.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Helpers;

namespace ClubDeportivo
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DebugMessage("Iniciando aplicación...");

                var dbHelper = new DatabaseHelper("ClubDB.sqlite");
                DebugMessage("Base de datos configurada");

                // Crear los repositorios
                var usuarioRepository = new UsuarioRepository(dbHelper);
                var socioRepository = new SocioRepository(dbHelper);
                var carnetRepository = new CarnetRepository(dbHelper);
                var pagoRepository = new PagoRepository(dbHelper);
                var actividadRepository = new ActividadRepository(dbHelper);
                var noSocioRepository = new NoSocioRepository(dbHelper);
                var cuotaRepository = new CuotaRepository(dbHelper);

                // Crear los servicios
                var authService = new AuthService(usuarioRepository);
                var socioService = new SocioService(socioRepository);
                var carnetService = new CarnetService(carnetRepository);
                var actividadService = new ActividadService(actividadRepository);
                var noSocioService = new NoSocioService(noSocioRepository);
                var pagoService = new PagoService(pagoRepository, actividadRepository, noSocioRepository);
                var cuotaService = new CuotaService(cuotaRepository, socioRepository);

                DebugMessage("Servicios creados");

                // Crear usuario admin por defecto si no existe
                CrearUsuarioAdminPorDefecto(usuarioRepository);

                // Mostrar primero el formulario de login
                var loginForm = new LoginForm(authService);
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    DebugMessage("Aplicación cerrada desde login");
                    return;
                }

                string nombreUsuario = authService.UsuarioActual != null
                    ? authService.UsuarioActual.NombreUsuario
                    : "Desconocido";
                DebugMessage("Usuario autenticado: " + nombreUsuario);

              

                // Pasar todos los servicios requeridos al formulario principal
                var mainForm = new MenuPrincipalForm(
                    socioService,
                    carnetService,
                    pagoService,
                    socioRepository,
                    noSocioService,
                    actividadService,
                    actividadRepository,
                    cuotaService,
                    cuotaRepository,
                    authService);

                mainForm.Shown += delegate (object s, EventArgs e)
                {
                    DebugMessage("Formulario principal visible");
                };

                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                ShowError("Error crítico: " + ex.Message);
            }
            finally
            {
                DebugMessage("Aplicación finalizada");
            }
        }

        private static void CrearUsuarioAdminPorDefecto(UsuarioRepository usuarioRepository)
        {
            try
            {
                const string usuarioAdmin = "admin";
                if (usuarioRepository.ObtenerPorNombreUsuario(usuarioAdmin) == null)
                {
                    var admin = new Usuario();
                    admin.FechaCreacion = DateTime.Now;

                    admin.NombreUsuario = usuarioAdmin;
                    admin.PasswordHash = HashHelper.HashPassword("admin123");
                    admin.Rol = "Admin";
                    admin.Activo = true;

                    usuarioRepository.Agregar(admin);
                    DebugMessage("Usuario admin creado por defecto");
                }
            }
            catch (Exception ex)
            {
                DebugMessage("Error al crear usuario admin: " + ex.Message);
            }
        }

        private static void DebugMessage(string message)
        {
#if DEBUG
            Console.WriteLine("[DEBUG] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + message);
#endif
        }

        private static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
