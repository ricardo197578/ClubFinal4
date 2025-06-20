using System.Collections.Generic;
using System.Linq;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Services
{
    public class ActividadService : IActividadService
    {
        private readonly IActividadRepository _repository;

        public ActividadService(IActividadRepository repository)
        {
            _repository = repository;
        }

        public List<Actividad> ObtenerActividadesDisponibles()
        {
            return _repository.ObtenerTodas();
        }

        public List<Actividad> ObtenerActividadesParaNoSocios()
        {
            return _repository.ObtenerTodas()
                .Where(a => !a.ExclusivaSocios)
                .ToList();
        }

        public Actividad ObtenerActividad(int id)
        {
            return _repository.ObtenerPorId(id);
        }
    }
}