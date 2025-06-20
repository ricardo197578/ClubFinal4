using System;
using System.Windows.Forms;
using ClubDeportivo.Services;
using ClubDeportivo.Repositories;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Views.Forms;

namespace ClubDeportivo.Views.Forms
{
    public class MenuPrincipalForm : Form
    {
        // Servicios y repositorios inyectados para acceso a funcionalidades
        private readonly ISocioService _socioService;
        private readonly ICarnetService _carnetService;
        private readonly IPagoService _pagoService;
        private readonly SocioRepository _socioRepository;
        private readonly INoSocioService _noSocioService;
        private readonly IActividadService _actividadService;
        private readonly ActividadRepository _actividadRepository;
        private readonly ICuotaService _cuotaService;
        private readonly ICuotaRepository _cuotaRepository;
        private readonly IAuthService _authService;

        // Constructor que recibe todas las dependencias necesarias
        public MenuPrincipalForm(
            ISocioService socioService,
            ICarnetService carnetService,
            IPagoService pagoService,
            SocioRepository socioRepository,
            INoSocioService noSocioService,
            IActividadService actividadService,
            ActividadRepository actividadRepository,
            ICuotaService cuotaService,
            ICuotaRepository cuotaRepository,
            IAuthService authService) // nuevo argumento para autenticación
        {
            // Asignar las dependencias a variables privadas para uso interno
            _socioService = socioService;
            _carnetService = carnetService;
            _pagoService = pagoService;
            _socioRepository = socioRepository;
            _noSocioService = noSocioService;
            _actividadService = actividadService;
            _actividadRepository = actividadRepository;
            _cuotaService = cuotaService;
            _cuotaRepository = cuotaRepository;
            _authService = authService; // almacenar para uso interno

            InitializeUI();  // Configurar la interfaz de usuario del formulario
        }

        private void InitializeUI()
        {
            // Configuración general del formulario
            this.Text = "Menú Principal - Club Minimal";
            this.Width = 350;
            this.Height = 420;
            this.StartPosition = FormStartPosition.CenterScreen; // Centrado al abrir
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // Tamaño fijo
            this.MaximizeBox = false;  // Deshabilitar maximizar ventana

            // Crear botones con texto y posición vertical (Top)
            var btnSocios = CreateButton("Registro de Socios", 30);
            var btnCarnetSocio = CreateButton("Entrega Carnet Socios", 70);
            var btnPagoCuota = CreateButton("Pago de Cuota Social", 110);
            var btnGestionCuotas = CreateButton("Vencimiento Cuotas Socios", 150);
            var btnNoSocios = CreateButton("Registro de No Socios", 190);
            var btnGestionActividades = CreateButton("Gestión de Actividades", 230);
            var btnPagoActividades = CreateButton("Pago Actividades No Socio", 270);
            var btnSalir = CreateButton("Salir", 340);

            // Asociar eventos Click a los botones para abrir formularios o realizar acciones

            btnSocios.Click += (s, e) => new SocioForm().ShowDialog(); // Abrir formulario Socios
            btnNoSocios.Click += (s, e) => new NoSocioForm().ShowDialog(); // Abrir formulario No Socios
            btnCarnetSocio.Click += (s, e) => new frmGestionCarnet(_socioService, _carnetService).ShowDialog(); // Gestión carnet
            // btnBuscarPorDni.Click += (s, e) => new frmBuscarSocioPorDni(_socioRepository).ShowDialog(); // Comentado, no usado
            btnPagoActividades.Click += (s, e) => new PagoActividadForm(
                                        _noSocioService,
                                        _actividadService,
                                        _pagoService).ShowDialog(); // Pago de actividades para no socios
            btnGestionActividades.Click += (s, e) => new frmActividad(_actividadRepository).ShowDialog(); // Gestión actividades
            btnPagoCuota.Click += (s, e) => new PagoCuotaForm(_cuotaService, _cuotaRepository).ShowDialog(); // Pago de cuota social
            btnGestionCuotas.Click += (s, e) => new GestionCuotasForm(_cuotaService).ShowDialog(); // Gestión de cuotas / vencimientos
            btnSalir.Click += (s, e) => this.Close(); // Cerrar el menú principal

            // Agregar todos los botones creados al formulario
            this.Controls.AddRange(new Control[] {
                btnSocios,
                btnNoSocios,
                btnCarnetSocio,
                btnGestionCuotas,
                btnPagoActividades,
                btnGestionActividades,
                btnPagoCuota,
                btnSalir
            });
        }

        // Método auxiliar para crear un botón con texto y posición vertical
        private Button CreateButton(string text, int top)
        {
            return new Button
            {
                Text = text,
                Left = 75,  // Posición horizontal fija
                Top = top,  // Posición vertical según parámetro
                Width = 200,
                Height = 30
            };
        }
    }
}
