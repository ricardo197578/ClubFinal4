using System;

namespace ClubDeportivo.Models
{
    public class Socio : Persona
    {
        public int Id { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public DateTime FechaVencimientoCuota { get; set; }
        public bool EstadoActivo { get; set; }
        public TipoSocio Tipo { get; set; } // por ahora un enum ver abajo

        
        public Socio()
        {
            EstadoActivo = true; 
        }

        // Constructor con parámetros
        public Socio(int id, string nombre, string apellido, string dni,
                    DateTime fechaInscripcion, DateTime fechaVencimiento, bool estadoActivo)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            FechaInscripcion = fechaInscripcion;
            FechaVencimientoCuota = fechaVencimiento;
            EstadoActivo = estadoActivo;
        }
    }

    public enum TipoSocio
    {
        Standard,
        Premium,
        Familiar
    }
}


