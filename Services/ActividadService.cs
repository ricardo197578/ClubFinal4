using System.Collections.Generic;
using System.Linq;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Services
{
    // Servicio que maneja la lógica de negocio relacionada con las actividades
    public class ActividadService : IActividadService
    {
        // Repositorio para acceder a los datos de actividades
        private readonly IActividadRepository _repository;

        // Constructor que recibe el repositorio como dependencia
        public ActividadService(IActividadRepository repository)
        {
            _repository = repository;
        }

        // Obtiene todas las actividades disponibles (sin filtro)
        public List<Actividad> ObtenerActividadesDisponibles()
        {
            return _repository.ObtenerTodas();
        }

        // Obtiene las actividades que NO son exclusivas para socios (para no socios)
        public List<Actividad> ObtenerActividadesParaNoSocios()
        {
            // Filtra las actividades donde ExclusivaSocios es falso
            return _repository.ObtenerTodas()
                .Where(a => !a.ExclusivaSocios)
                .ToList();
        }

        // Obtiene una actividad específica por su Id
        public Actividad ObtenerActividad(int id)
        {
            return _repository.ObtenerPorId(id);
        }
    }
}
