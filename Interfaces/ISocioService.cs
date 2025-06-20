using System;
using System.Collections.Generic;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    //Es una interface, por lo que solo declara métodos sin implementarlos
    public interface ISocioService
    {
        void RegistrarSocio(string nombre, string apellido, string dni);
        List<Socio> ObtenerSocios();
        Socio GetSocio(int id);
        Socio GetSocio(string dni);
              
        //EXPLICACION DE AGREGADO DE METODOS
        bool ExisteDni(string dni); // 1RO AGREGO EL METODO PARA VALIDAR PRIMERO 

        //por las dudas eliminar
        void EliminarSocio(int id);

    }
}

