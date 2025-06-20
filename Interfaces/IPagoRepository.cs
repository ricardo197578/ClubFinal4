// Importa funcionalidades básicas del sistema, como tipos primitivos y estructuras de fecha y hora
using System;

// Importa el modelo 'Pago' definido en el proyecto
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para las operaciones relacionadas con el registro de pagos
    public interface IPagoRepository
    {
        // Registra un nuevo pago en el sistema
        void RegistrarPago(Pago pago);
    }
}
