// Archivo: Actividad.cs
using System;

namespace ClubDeportivo.Models
{
    // Clase que representa una actividad deportiva en el club
    public class Actividad
    {
        public int Id { get; set; } // Identificador �nico de la actividad
        public string Nombre { get; set; } // Nombre de la actividad
        public string Descripcion { get; set; } // Descripci�n detallada
        public string Horario { get; set; } // Horarios de realizaci�n
        public decimal Precio { get; set; } // Costo de la actividad
        public bool ExclusivaSocios { get; set; } // Si es solo para socios
    }
}