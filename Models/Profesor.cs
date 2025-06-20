using System;
using System.Collections.Generic;

namespace ClubDeportivo.Models
{
    public class Profesor : Persona
    {
        public string Legajo { get; set; }
        public DateTime FechaContratacion { get; set; }
        public bool EsTitular { get; set; }
        public List<Actividad> Actividades { get; set; }

       

        

       
    }
}
