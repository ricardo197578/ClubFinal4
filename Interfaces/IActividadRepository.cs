// Importa el espacio de nombres necesario para usar listas gen�ricas
using System.Collections.Generic;

// Importa los modelos definidos en el proyecto, incluyendo la clase Actividad
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz que establece el contrato para un repositorio de actividades
    public interface IActividadRepository
    {
        // M�todo para agregar una nueva actividad al repositorio
        void Agregar(Actividad actividad);

        // M�todo para obtener la lista completa de todas las actividades registradas
        List<Actividad> ObtenerTodas();

        // M�todo para obtener una actividad espec�fica a partir de su identificador �nico (ID)
        Actividad ObtenerPorId(int id);
    }
}
