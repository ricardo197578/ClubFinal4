using System.Collections.Generic;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    public interface INoSocioService
    {
        void RegistrarNoSocio(string nombre, string apellido,string dni);
        List<NoSocio> ObtenerNoSocios();
        NoSocio BuscarPorDni(string dni); 
    }
}