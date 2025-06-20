// Archivo: Suplente.cs
using System;

namespace ClubDeportivo.Models
{
    // Clase que representa un profesor suplente
    public class Suplente : Profesor
    {
        public DateTime FechaDisponibilidad { get; set; } // Fechas disponibles
    }
}