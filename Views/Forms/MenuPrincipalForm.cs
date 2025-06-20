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

        //Constructor
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
            IAuthService authService) // nuevo argumento) 
        {
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

            InitializeUI();
        }

        private void InitializeUI()
        {
            // Configuración del formulario
            this.Text = "Menú Principal - Club Minimal";
            this.Width = 350;
            this.Height = 420; 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Configuración de controles 
            var btnSocios = CreateButton("Registro de Socios", 30);
            var btnCarnetSocio = CreateButton("Entrega Carnet Socios", 70);
            var btnPagoCuota = CreateButton("Pago de Cuota Social", 110);
            var btnGestionCuotas = CreateButton("Vencimiento Cuotas Socios", 150);
            var btnNoSocios = CreateButton("Registro de No Socios", 190);
            var btnGestionActividades = CreateButton("Gestión de Actividades", 230);          
            var btnPagoActividades = CreateButton("Pago Actividades No Socio", 270);
            



            var btnSalir = CreateButton("Salir", 340); 

            // Event handlers
            btnSocios.Click += (s, e) => new SocioForm().ShowDialog();
            btnNoSocios.Click += (s, e) => new NoSocioForm().ShowDialog();
            btnCarnetSocio.Click += (s, e) => new frmGestionCarnet(_socioService, _carnetService).ShowDialog();
            //btnBuscarPorDni.Click += (s, e) => new frmBuscarSocioPorDni(_socioRepository).ShowDialog();
            btnPagoActividades.Click += (s, e) => new PagoActividadForm(
                                        _noSocioService,
                                        _actividadService,
                                        _pagoService).ShowDialog();
            btnGestionActividades.Click += (s, e) => new frmActividad(_actividadRepository).ShowDialog();
            btnPagoCuota.Click += (s, e) => new PagoCuotaForm(_cuotaService, _cuotaRepository).ShowDialog();
            btnGestionCuotas.Click += (s, e) => new GestionCuotasForm(_cuotaService).ShowDialog();
            btnSalir.Click += (s, e) => this.Close();

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

        private Button CreateButton(string text, int top)
        {
            return new Button
            {
                Text = text,
                Left = 75,
                Top = top,
                Width = 200,
                Height = 30
            };
        }
    }
}
