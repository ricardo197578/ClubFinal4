using System;
using System.Collections.Generic;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    public interface ISocioRepository
    {
        void Agregar(Socio socio);
        List<Socio> ObtenerTodos();
        Socio ObtenerPorId(int id);
        Socio ObtenerPorDni(string Dni);
        //por las dudas 
        void Eliminar(int id);
    }
}