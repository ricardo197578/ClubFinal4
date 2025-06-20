// Importa el espacio de nombres necesario para trabajar con listas gen�ricas
using System.Collections.Generic;

// Importa el modelo 'NoSocio' definido en el proyecto
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para los servicios relacionados con la gesti�n de no socios
    public interface INoSocioService
    {
        // Registra un nuevo no socio con nombre, apellido y DNI
        void RegistrarNoSocio(string nombre, string apellido, string dni);

        // Devuelve una lista con todos los no socios registrados
        List<NoSocio> ObtenerNoSocios();

        // Busca y devuelve un no socio a partir de su n�mero de documento (DNI)
        NoSocio BuscarPorDni(string dni);
    }
}
