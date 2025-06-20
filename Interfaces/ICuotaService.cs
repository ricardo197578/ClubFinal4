using System;
using ClubDeportivo.Models;
using System.Collections.Generic;

namespace ClubDeportivo.Interfaces
{
    public interface ICuotaService
    {
        Socio BuscarSocio(string dni);
        void ProcesarPago(int socioId, decimal monto, MetodoPago metodo);
        decimal ObtenerValorCuota();
        Socio BuscarSocioPorId(int socioId);

        IEnumerable<Socio> ObtenerTodosSocios();
        IEnumerable<Socio> ObtenerSociosConCuotasVencidas(DateTime fechaConsulta);
    }
}
