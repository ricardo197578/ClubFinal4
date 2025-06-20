// Archivo: Profesor.cs
using System;
using System.Collections.Generic;

namespace ClubDeportivo.Models
{
    // Clase que representa a un profesor del club
    public class Profesor : Persona
    {
        public string Legajo { get; set; } // Número de legajo
        public DateTime FechaContratacion { get; set; } // Fecha de ingreso
        public bool EsTitular { get; set; } // Si es titular o suplente
        public List<Actividad> Actividades { get; set; } // Actividades que dicta
    }
}