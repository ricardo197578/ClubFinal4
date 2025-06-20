using System;

using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    public interface IPagoRepository
    {
        void RegistrarPago(Pago pago);
    }
}