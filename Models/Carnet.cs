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
            AptoFisico = false; // Por defecto no tiene apto f�sico
            FechaEmision = DateTime.Now; // Fecha actual de emisi�n
            FechaVencimiento = DateTime.Now.AddYears(1); // Vence en 1 a�o
        }

        public int Id { get; set; } // Identificador �nico
        public int NroCarnet { get; set; } // N�mero de carnet visible
        public DateTime FechaEmision { get; set; } // Fecha de emisi�n
        public DateTime FechaVencimiento { get; set; } // Fecha de vencimiento
        public bool AptoFisico { get; set; } // Estado del apto f�sico
        public int SocioId { get; set; } // ID del socio asociado
    }
}