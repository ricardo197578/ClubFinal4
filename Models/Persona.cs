// Archivo: Persona.cs
using System;

namespace ClubDeportivo.Models
{
    // Clase base que representa una persona
    public class Persona
    {
        public string Nombre { get; set; } // Nombre(s)
        public string Apellido { get; set; } // Apellido(s)
        public string Dni { get; set; } // Documento Nacional de Identidad
        public DateTime FechaNacimiento { get; set; } // Fecha de nacimiento
        public string Direccion { get; set; } // Dirección particular
        public string Telefono { get; set; } // Teléfono de contacto
        public string Email { get; set; } // Correo electrónico
    }
}