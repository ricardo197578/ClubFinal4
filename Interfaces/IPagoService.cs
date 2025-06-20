// Importa funcionalidades básicas del sistema como tipos primitivos y estructuras de fecha y hora
using System;

// Importa el modelo 'MetodoPago' y posiblemente otros modelos definidos en el proyecto
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para los servicios relacionados con el procesamiento de pagos
    public interface IPagoService
    {
        // Procesa un pago realizado por un no socio para una actividad específica,
        // incluyendo el monto pagado y el método de pago utilizado
        void ProcesarPago(int noSocioId, int actividadId, decimal monto, MetodoPago metodo);
    }
}
