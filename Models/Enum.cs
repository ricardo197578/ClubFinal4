// Archivo: MetodoPago.cs
using System;

namespace ClubDeportivo.Models
{
    // Enumeraci�n de los m�todos de pago disponibles
    public enum MetodoPago
    {
        EFECTIVO,
        TARJETA_CREDITO,
        TRANSFERENCIA,
        DEBITO_AUTOMATICO,
        MERCADO_PAGO
    }
}