using System;

using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    public interface IPagoService
    {
        void ProcesarPago(int noSocioId, int actividadId, decimal monto, MetodoPago metodo);
    }
}