using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Services
{
    // Servicio para manejar la lógica relacionada con las cuotas de socios
    public class CuotaService : ICuotaService
    {
        // Repositorios para cuotas y socios
        private readonly ICuotaRepository _cuotaRepository;
        private readonly ISocioRepository _socioRepository;

        // Valor fijo de la cuota
        private const decimal _valorCuota = 5000.00m;

        // Constructor que valida e inyecta dependencias
        public CuotaService(ICuotaRepository cuotaRepository, ISocioRepository socioRepository)
        {
            if (cuotaRepository == null)
                throw new ArgumentNullException("cuotaRepository");
            if (socioRepository == null)
                throw new ArgumentNullException("socioRepository");

            _cuotaRepository = cuotaRepository;
            _socioRepository = socioRepository;
        }

        // Busca un socio activo por su DNI (sin espacios)
        public Socio BuscarSocio(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío");

            return _cuotaRepository.BuscarSocioActivoPorDni(dni.Trim());
        }

        // Procesa el pago de la cuota para un socio con monto y método especificados
        public void ProcesarPago(int socioId, decimal monto, MetodoPago metodo)
        {
            // Verifica que el monto sea exactamente el valor definido para la cuota
            if (monto != _valorCuota)
                throw new ArgumentException(string.Format("El monto debe ser exactamente {0:C}", _valorCuota));

            // Utiliza una transacción para asegurar la atomicidad del registro del pago
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _cuotaRepository.RegistrarPagoCuota(
                        socioId,
                        monto,
                        DateTime.Now,
                        metodo);

                    transaction.Complete(); // Marca la transacción como exitosa
                }
                catch
                {
                    transaction.Dispose(); // Desecha la transacción en caso de error
                    throw;
                }
            }
        }

        // Retorna el valor fijo de la cuota
        public decimal ObtenerValorCuota()
        {
            return _valorCuota;
        }

        // Obtiene cuotas próximas a vencer hasta una fecha límite, ordenadas por fecha de vencimiento
        public IEnumerable<Cuota> ObtenerCuotasPorVencer(DateTime fechaLimite)
        {
            return _cuotaRepository.ObtenerCuotasPorVencer(fechaLimite)
                .OrderBy(function => function.FechaVencimiento)
                .ToList();
        }

        // Obtiene todas las cuotas de un socio ordenadas de la más reciente a la más antigua
        public IEnumerable<Cuota> ObtenerCuotasPorSocio(int socioId)
        {
            return _cuotaRepository.ObtenerCuotasPorSocio(socioId)
                .OrderByDescending(function => function.FechaVencimiento)
                .ToList();
        }

        // Obtiene socios que tienen cuotas vencidas hasta una fecha y que están activos, ordenados por fecha y apellido
        public IEnumerable<Socio> ObtenerSociosConCuotasVencidas(DateTime fechaConsulta)
        {
            return _socioRepository.ObtenerTodos()
                .Where(function => function.FechaVencimientoCuota <= fechaConsulta && function.EstadoActivo)
                .OrderBy(function => function.FechaVencimientoCuota)
                .ThenBy(function => function.Apellido)
                .ToList();
        }

        // Obtiene todos los socios activos ordenados por apellido y nombre
        public IEnumerable<Socio> ObtenerTodosSocios()
        {
            return _socioRepository.ObtenerTodos()
                .OrderBy(function => function.Apellido)
                .ThenBy(function => function.Nombre)
                .ToList();
        }

        // Busca un socio por su Id
        public Socio BuscarSocioPorId(int socioId)
        {
            return _socioRepository.ObtenerPorId(socioId);
        }

        // Calcula la nueva fecha de vencimiento de la cuota según el tipo de socio
        private DateTime CalcularNuevoVencimiento(int socioId)
        {
            var socio = _socioRepository.ObtenerPorId(socioId);
            if (socio == null)
                throw new KeyNotFoundException("Socio no encontrado");

            if (socio.Tipo == TipoSocio.Premium)
                return DateTime.Now.AddMonths(2);
            else if (socio.Tipo == TipoSocio.Familiar)
                return DateTime.Now.AddMonths(3);
            else
                return DateTime.Now.AddMonths(1);
        }
    }
}

