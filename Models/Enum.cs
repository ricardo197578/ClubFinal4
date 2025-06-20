// Archivo: MetodoPago.cs
using System;

namespace ClubDeportivo.Models
{
    // Enumeración de los métodos de pago disponibles
    public enum MetodoPago
    {
        EFECTIVO,
        TARJETA_CREDITO,
        TRANSFERENCIA,
        DEBITO_AUTOMATICO,
        MERCADO_PAGO
    }
}