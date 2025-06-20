using System;

namespace ClubDeportivo.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int NoSocioId { get; set; }
        public int ActividadId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public MetodoPago Metodo { get; set; }
    }
  
}