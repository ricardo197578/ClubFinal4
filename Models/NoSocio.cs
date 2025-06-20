// Archivo: NoSocio.cs
using System;

namespace ClubDeportivo.Models
{
    // Clase que representa a un no socio (persona ocasional)
    public class NoSocio : Persona
    {
        public int Id { get; set; } // Identificador único
        public DateTime FechaRegistro { get; set; } // Fecha de registro
    }
}