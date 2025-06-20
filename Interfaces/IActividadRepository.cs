using System.Collections.Generic;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    public interface IActividadRepository
    {
        void Agregar(Actividad actividad);
        List<Actividad> ObtenerTodas();
        Actividad ObtenerPorId(int id);
    }
}