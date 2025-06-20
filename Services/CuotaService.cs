using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Services
{
    public class CuotaService : ICuotaService
    {
        private readonly ICuotaRepository _cuotaRepository;
        private readonly ISocioRepository _socioRepository;
        private const decimal _valorCuota = 5000.00m;

        public CuotaService(ICuotaRepository cuotaRepository, ISocioRepository socioRepository)
        {
            if (cuotaRepository == null)
                throw new ArgumentNullException("cuotaRepository");
            if (socioRepository == null)
                throw new ArgumentNullException("socioRepository");

            _cuotaRepository = cuotaRepository;
            _socioRepository = socioRepository;
        }

        public Socio BuscarSocio(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío");

            return _cuotaRepository.BuscarSocioActivoPorDni(dni.Trim());
        }

        public void ProcesarPago(int socioId, decimal monto, MetodoPago metodo)
        {
            if (monto != _valorCuota)
                throw new ArgumentException(string.Format("El monto debe ser exactamente {0:C}", _valorCuota));

            using (var transaction = new TransactionScope())
            {
                try
                {
                    _cuotaRepository.RegistrarPagoCuota(
                        socioId,
                        monto,
                        DateTime.Now,
                        metodo);

                    transaction.Complete();
                }
                catch
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public decimal ObtenerValorCuota()
        {
            return _valorCuota;
        }

        public IEnumerable<Cuota> ObtenerCuotasPorVencer(DateTime fechaLimite)
        {
            return _cuotaRepository.ObtenerCuotasPorVencer(fechaLimite)
                .OrderBy(function => function.FechaVencimiento)
                .ToList();
        }

        public IEnumerable<Cuota> ObtenerCuotasPorSocio(int socioId)
        {
            return _cuotaRepository.ObtenerCuotasPorSocio(socioId)
                .OrderByDescending(function => function.FechaVencimiento)
                .ToList();
        }

        public IEnumerable<Socio> ObtenerSociosConCuotasVencidas(DateTime fechaConsulta)
        {
            return _socioRepository.ObtenerTodos()
                .Where(function => function.FechaVencimientoCuota <= fechaConsulta && function.EstadoActivo)
                .OrderBy(function => function.FechaVencimientoCuota)
                .ThenBy(function => function.Apellido)
                .ToList();
        }

        public IEnumerable<Socio> ObtenerTodosSocios()
        {
            return _socioRepository.ObtenerTodos()
                .OrderBy(function => function.Apellido)
                .ThenBy(function => function.Nombre)
                .ToList();
        }

        public Socio BuscarSocioPorId(int socioId)
        {
            return _socioRepository.ObtenerPorId(socioId);
        }

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
