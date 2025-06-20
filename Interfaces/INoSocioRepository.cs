using System.Collections.Generic;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    public interface INoSocioRepository
    {
        void Agregar(NoSocio noSocio);
        List<NoSocio> ObtenerTodos();

       
        NoSocio ObtenerPorId(int id);
        NoSocio BuscarPorDni(string dni);
    }
}