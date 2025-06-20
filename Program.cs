//CODIGO COMENTADO POR IA
using System;
using System.Windows.Forms;
using ClubDeportivo.Repositories;
using ClubDeportivo.Services;
using ClubDeportivo.Views.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Helpers;

namespace ClubDeportivo
{
    /// <summary>
    /// Clase principal que contiene el punto de entrada de la aplicación.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Atributo STAThread requerido para aplicaciones Windows Forms
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configuración inicial de la aplicación Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Registro de depuración
                DebugMessage("Iniciando aplicación...");

                // Creación del helper para la base de datos SQLite
                var dbHelper = new DatabaseHelper("ClubDB.sqlite");
                DebugMessage("Base de datos configurada");

                // ---------- INICIALIZACIÓN DE REPOSITORIOS ----------
                // Creación de todas las instancias de repositorios para acceso a datos
                var usuarioRepository = new UsuarioRepository(dbHelper);
                var socioRepository = new SocioRepository(dbHelper);
                var carnetRepository = new CarnetRepository(dbHelper);
                var pagoRepository = new PagoRepository(dbHelper);
                var actividadRepository = new ActividadRepository(dbHelper);
                var noSocioRepository = new NoSocioRepository(dbHelper);
                var cuotaRepository = new CuotaRepository(dbHelper);

                // ---------- INICIALIZACIÓN DE SERVICIOS ----------
                // Creación de los servicios de negocio que usarán los repositorios
                var authService = new AuthService(usuarioRepository);
                var socioService = new SocioService(socioRepository);
                var carnetService = new CarnetService(carnetRepository);
                var actividadService = new ActividadService(actividadRepository);
                var noSocioService = new NoSocioService(noSocioRepository);
                var pagoService = new PagoService(pagoRepository, actividadRepository, noSocioRepository);
                var cuotaService = new CuotaService(cuotaRepository, socioRepository);

                DebugMessage("Servicios creados");

                // Creación del usuario administrador por defecto si no existe
                CrearUsuarioAdminPorDefecto(usuarioRepository);

                // ---------- FLUJO PRINCIPAL DE LA APLICACIÓN ----------
                // Mostrar el formulario de login
                var loginForm = new LoginForm(authService);
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    DebugMessage("Aplicación cerrada desde login");
                    return; // Salir si el login no fue exitoso
                }

                // Obtener nombre del usuario autenticado
                string nombreUsuario = authService.UsuarioActual != null
                    ? authService.UsuarioActual.NombreUsuario
                    : "Desconocido";
                DebugMessage("Usuario autenticado: " + nombreUsuario);

                // ---------- FORMULARIO PRINCIPAL ----------
                // Crear y mostrar el formulario principal con todos los servicios necesarios
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

                // Evento para registrar cuando el formulario principal se muestra
                mainForm.Shown += delegate (object s, EventArgs e)
                {
                    DebugMessage("Formulario principal visible");
                };

                // Ejecutar la aplicación con el formulario principal
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                // Manejo de errores global de la aplicación
                ShowError("Error crítico: " + ex.Message);
            }
            finally
            {
                // Mensaje final de depuración
                DebugMessage("Aplicación finalizada");
            }
        }

        /// <summary>
        /// Crea un usuario administrador por defecto si no existe en la base de datos
        /// </summary>
        /// <param name="usuarioRepository">Repositorio de usuarios</param>
        private static void CrearUsuarioAdminPorDefecto(UsuarioRepository usuarioRepository)
        {
            try
            {
                const string usuarioAdmin = "admin";

                // Verificar si el usuario admin ya existe
                if (usuarioRepository.ObtenerPorNombreUsuario(usuarioAdmin) == null)
                {
                    // Crear nuevo usuario admin
                    var admin = new Usuario();
                    admin.FechaCreacion = DateTime.Now;
                    admin.NombreUsuario = usuarioAdmin;
                    admin.PasswordHash = HashHelper.HashPassword("admin123"); // Contraseña por defecto hasheada
                    admin.Rol = "Admin";
                    admin.Activo = true;

                    // Guardar en la base de datos
                    usuarioRepository.Agregar(admin);
                    DebugMessage("Usuario admin creado por defecto");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores al crear el usuario admin
                DebugMessage("Error al crear usuario admin: " + ex.Message);
            }
        }

        /// <summary>
        /// Muestra un mensaje de depuración en la consola (solo en modo DEBUG)
        /// </summary>
        /// <param name="message">Mensaje a mostrar</param>
        private static void DebugMessage(string message)
        {
#if DEBUG
            Console.WriteLine("[DEBUG] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + message);
#endif
        }

        /// <summary>
        /// Muestra un mensaje de error al usuario
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        private static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

/********************************
  Estructura y Funcionalidad Clave
Inicialización de la Aplicación:

Configuración básica de Windows Forms

Creación de la cadena de dependencias (repositorios → servicios)

Patrones de Diseño:

Inyección de Dependencias: Todos los servicios reciben sus repositorios

Separación de preocupaciones: Repositorios (datos) vs Servicios (lógica)

Flujo de la Aplicación:

Configuración inicial

Creación de usuario admin si no existe

Mostrar formulario de login

Si login es exitoso, mostrar formulario principal

Manejo de Errores:

Try-catch global para errores no controlados

Mensajes de depuración en consola (solo en DEBUG)

Seguridad:

Creación de usuario admin con contraseña hasheada

Validación de autenticación antes de mostrar pantalla principal

Características Destacables
Configuración Modular:

Cada repositorio y servicio se inicializa separadamente

Fácil de mantener y extender

Mensajes de Depuración:

Útiles para desarrollo y troubleshooting

Solo activos en compilación DEBUG

Usuario por Defecto:

Garantiza que siempre haya un usuario admin para acceder al sistema

Contraseña almacenada de forma segura (hasheada)

Arquitectura:

Claramente separada en capas

Fácil de testear debido a la inyección de dependencias

Este código representa el punto de entrada bien estructurado de una aplicación Windows Forms con una arquitectura limpia y mantenible.
 
 ********************************/