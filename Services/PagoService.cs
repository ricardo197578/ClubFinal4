using System;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly IActividadRepository _actividadRepository;
        private readonly INoSocioRepository _noSocioRepository;

        public PagoService(
            IPagoRepository pagoRepository,
            IActividadRepository actividadRepository,
            INoSocioRepository noSocioRepository)
        {
            _pagoRepository = pagoRepository;
            _actividadRepository = actividadRepository;
            _noSocioRepository = noSocioRepository;
        }

        public void ProcesarPago(int noSocioId, int actividadId, decimal monto, MetodoPago metodo)
        {
            // Validar que exista el no socio
            var noSocio = _noSocioRepository.ObtenerPorId(noSocioId);
            if (noSocio == null)
                throw new System.Exception("No Socio no encontrado");

            // Validar que exista la actividad
            var actividad = _actividadRepository.ObtenerPorId(actividadId);
            if (actividad == null)
                throw new System.Exception("Actividad no encontrada");

            // Validar que el monto coincida
            if (actividad.Precio != monto)
                throw new System.Exception("El monto no coincide con el precio de la actividad");

            // Registrar el pago
            var pago = new Pago
            {
                NoSocioId = noSocioId,
                ActividadId = actividadId,
                Monto = monto,
                FechaPago = DateTime.Now,
                Metodo = metodo
            };

            _pagoRepository.RegistrarPago(pago);
        }
    }
}