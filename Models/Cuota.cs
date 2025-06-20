// Archivo: Cuota.cs
using System;

namespace ClubDeportivo.Models
{
    // Clase que representa las cuotas de los socios
    public class Cuota
    {
        public int Id { get; set; } // Identificador único
        public int SocioId { get; set; } // ID del socio asociado
        public decimal Monto { get; set; } // Valor de la cuota
        public DateTime FechaPago { get; set; } // Fecha de pago
        public DateTime FechaVencimiento { get; set; } // Fecha de vencimiento
        public bool Pagada { get; set; } // Estado de pago
        public MetodoPago Metodo { get; set; } // Forma de pago (enum)
        public string Periodo { get; set; } // Periodo en formato YYYY-MM

        // Constructor vacío
        public Cuota() { }

        // Constructor completo
        public Cuota(int id, int socioId, decimal monto, DateTime fechaPago,
                    DateTime fechaVencimiento, bool pagada, MetodoPago metodo)
        {
            Id = id;
            SocioId = socioId;
            Monto = monto;
            FechaPago = fechaPago;
            FechaVencimiento = fechaVencimiento;
            Pagada = pagada;
            Metodo = metodo;
            Periodo = fechaVencimiento.ToString("yyyy-MM");
        }
    }
}