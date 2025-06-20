// Importa funcionalidades básicas del sistema, incluyendo tipos como DateTime
using System;

// Importa el modelo 'Socio' y el enumerador 'MetodoPago' definidos en el proyecto
using ClubDeportivo.Models;

// Importa el espacio de nombres necesario para trabajar con colecciones genéricas
using System.Collections.Generic;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para los servicios relacionados con la gestión de cuotas
    public interface ICuotaService
    {
        // Busca y devuelve un socio a partir de su número de documento (DNI)
        Socio BuscarSocio(string dni);

        // Procesa el pago de una cuota para un socio específico, con el monto y el método de pago indicado
        void ProcesarPago(int socioId, decimal monto, MetodoPago metodo);

        // Devuelve el valor actual de una cuota (por ejemplo, el valor estándar mensual)
        decimal ObtenerValorCuota();

        // Busca un socio por su identificador único (ID)
        Socio BuscarSocioPorId(int socioId);

        // Devuelve una lista de todos los socios registrados
        IEnumerable<Socio> ObtenerTodosSocios();

        // Devuelve una lista de socios que tienen cuotas vencidas a la fecha indicada
        IEnumerable<Socio> ObtenerSociosConCuotasVencidas(DateTime fechaConsulta);
    }
}
