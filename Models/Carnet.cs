// Archivo: Carnet.cs
using System;

namespace ClubDeportivo.Models
{
    // Clase que representa el carnet de socio
    public class Carnet
    {
        // Constructor que inicializa valores por defecto
        public Carnet()
        {
            AptoFisico = false; // Por defecto no tiene apto físico
            FechaEmision = DateTime.Now; // Fecha actual de emisión
            FechaVencimiento = DateTime.Now.AddYears(1); // Vence en 1 año
        }

        public int Id { get; set; } // Identificador único
        public int NroCarnet { get; set; } // Número de carnet visible
        public DateTime FechaEmision { get; set; } // Fecha de emisión
        public DateTime FechaVencimiento { get; set; } // Fecha de vencimiento
        public bool AptoFisico { get; set; } // Estado del apto físico
        public int SocioId { get; set; } // ID del socio asociado
    }
}