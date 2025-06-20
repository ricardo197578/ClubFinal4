// Importa funcionalidades b�sicas del sistema, incluyendo tipos como DateTime
using System;

// Importa el modelo 'Socio' y el enumerador 'MetodoPago' definidos en el proyecto
using ClubDeportivo.Models;

// Importa el espacio de nombres necesario para trabajar con colecciones gen�ricas
using System.Collections.Generic;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para los servicios relacionados con la gesti�n de cuotas
    public interface ICuotaService
    {
        // Busca y devuelve un socio a partir de su n�mero de documento (DNI)
        Socio BuscarSocio(string dni);

        // Procesa el pago de una cuota para un socio espec�fico, con el monto y el m�todo de pago indicado
        void ProcesarPago(int socioId, decimal monto, MetodoPago metodo);

        // Devuelve el valor actual de una cuota (por ejemplo, el valor est�ndar mensual)
        decimal ObtenerValorCuota();

        // Busca un socio por su identificador �nico (ID)
        Socio BuscarSocioPorId(int socioId);

        // Devuelve una lista de todos los socios registrados
        IEnumerable<Socio> ObtenerTodosSocios();

        // Devuelve una lista de socios que tienen cuotas vencidas a la fecha indicada
        IEnumerable<Socio> ObtenerSociosConCuotasVencidas(DateTime fechaConsulta);
    }
}
