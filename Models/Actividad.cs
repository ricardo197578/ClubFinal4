using System;

namespace ClubDeportivo.Models
{
    public class Actividad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Horario { get; set; }
        public decimal Precio { get; set; }
        public bool ExclusivaSocios { get; set; }
    }
}