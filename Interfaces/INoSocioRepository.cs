// Importa el espacio de nombres necesario para trabajar con listas gen�ricas
using System.Collections.Generic;

// Importa el modelo 'NoSocio' definido en el proyecto
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para las operaciones que se pueden realizar sobre los no socios
    public interface INoSocioRepository
    {
        // Agrega un nuevo no socio al repositorio
        void Agregar(NoSocio noSocio);

        // Devuelve una lista con todos los no socios registrados
        List<NoSocio> ObtenerTodos();

        // Devuelve un no socio espec�fico seg�n su identificador �nico (ID)
        NoSocio ObtenerPorId(int id);

        // Busca y devuelve un no socio seg�n su n�mero de documento (DNI)
        NoSocio BuscarPorDni(string dni);
    }
}
