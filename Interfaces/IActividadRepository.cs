// Importa el espacio de nombres necesario para usar listas genéricas
using System.Collections.Generic;

// Importa los modelos definidos en el proyecto, incluyendo la clase Actividad
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz que establece el contrato para un repositorio de actividades
    public interface IActividadRepository
    {
        // Método para agregar una nueva actividad al repositorio
        void Agregar(Actividad actividad);

        // Método para obtener la lista completa de todas las actividades registradas
        List<Actividad> ObtenerTodas();

        // Método para obtener una actividad específica a partir de su identificador único (ID)
        Actividad ObtenerPorId(int id);
    }
}
