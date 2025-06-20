using System;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Services
{
    // Servicio que gestiona la lógica de negocio para el procesamiento de pagos
    public class PagoService : IPagoService
    {
        // Repositorios necesarios para pagos, actividades y no socios
        private readonly IPagoRepository _pagoRepository;
        private readonly IActividadRepository _actividadRepository;
        private readonly INoSocioRepository _noSocioRepository;

        // Constructor que recibe los repositorios a través de inyección de dependencias
        public PagoService(
            IPagoRepository pagoRepository,
            IActividadRepository actividadRepository,
            INoSocioRepository noSocioRepository)
        {
            _pagoRepository = pagoRepository;
            _actividadRepository = actividadRepository;
            _noSocioRepository = noSocioRepository;
        }

        // Método para procesar un pago dado un no socio, actividad, monto y método de pago
        public void ProcesarPago(int noSocioId, int actividadId, decimal monto, MetodoPago metodo)
        {
            // Validar que el no socio exista
            var noSocio = _noSocioRepository.ObtenerPorId(noSocioId);
            if (noSocio == null)
                throw new System.Exception("No Socio no encontrado");

            // Validar que la actividad exista
            var actividad = _actividadRepository.ObtenerPorId(actividadId);
            if (actividad == null)
                throw new System.Exception("Actividad no encontrada");

            // Validar que el monto coincida con el precio de la actividad
            if (actividad.Precio != monto)
                throw new System.Exception("El monto no coincide con el precio de la actividad");

            // Crear el objeto pago con los datos correspondientes
            var pago = new Pago
            {
                NoSocioId = noSocioId,
                ActividadId = actividadId,
                Monto = monto,
                FechaPago = DateTime.Now,
                Metodo = metodo
            };

            // Registrar el pago en la base de datos mediante el repositorio
            _pagoRepository.RegistrarPago(pago);
        }
    }
}
