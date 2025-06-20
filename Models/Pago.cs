// Archivo: Pago.cs
using System;

namespace ClubDeportivo.Models
{
    // Clase que representa los pagos de no socios
    public class Pago
    {
        public int Id { get; set; } // Identificador único
        public int NoSocioId { get; set; } // ID del no socio
        public int ActividadId { get; set; } // ID de la actividad
        public decimal Monto { get; set; } // Monto pagado
        public DateTime FechaPago { get; set; } // Fecha de pago
        public MetodoPago Metodo { get; set; } // Método de pago
    }
}