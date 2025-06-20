using System;

namespace ClubDeportivo.Models
{
    public class Cuota
    {
        public int Id { get; set; }
        public int SocioId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Pagada { get; set; }
        public MetodoPago Metodo { get; set; }
        public string Periodo { get; set; } 

        
        public Cuota() { }

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