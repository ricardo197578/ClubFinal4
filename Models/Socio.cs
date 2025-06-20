using System;

namespace ClubDeportivo.Models
{
    // Clase Socio que hereda de Persona
    public class Socio : Persona
    {
        // Propiedades específicas de Socio
        public int Id { get; set; }//Identificador unico
        public DateTime FechaInscripcion { get; set; }
        public DateTime FechaVencimientoCuota { get; set; }
        public bool EstadoActivo { get; set; }
        public TipoSocio Tipo { get; set; } // Enum que define tipos de socio

        // Constructor por defecto
        public Socio()
        {
            EstadoActivo = true; // Los socios se crean como activos por defecto
        }

        // Constructor con parámetros
        public Socio(int id, string nombre, string apellido, string dni,
                    DateTime fechaInscripcion, DateTime fechaVencimiento, bool estadoActivo)
        {
            Id = id;//Heredado
            Nombre = nombre;//Heredado
            Apellido = apellido;//Heredado
            Dni = dni;//Heredado
            FechaInscripcion = fechaInscripcion;
            FechaVencimientoCuota = fechaVencimiento;
            EstadoActivo = estadoActivo;
        }
    }

    // Definición del enum TipoSocio
    public enum TipoSocio
    {
        Standard,//Socio Basico
        Premium,//Socio con beneficios adicionales
        Familiar//Socio que incluye familiares
    }
}


