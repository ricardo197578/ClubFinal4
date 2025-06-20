//operaciones logicas de negocio lo que debe hacer el sistema
using System;
using System.Collections.Generic;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    //Es una interface, por lo que solo declara métodos sin implementarlos
    public interface ISocioService
    {
        //Metodos para Gestion de Socios
        //void RegistrarSocio(string nombre, string apellido, string dni, TipoSocio tipo);//Registrar un nuevo socio en el sistema
        void RegistrarSocio(Socio socio);
        List<Socio> ObtenerSocios();//Obtener todos los socios registrados,retorna lista de objeto Socio
        Socio GetSocio(int id);//Obtener un socio por su id (parametro )unico retorna un objeto Socio o null si no existe
        Socio GetSocio(string dni);//Obtener un socio por su Dni (parametro)  retorna un objeto Socio o null si no existe
              
        //Metodos para validacion y eliminacion
        bool ExisteDni(string dni); // 1RO AGREGO EL METODO PARA VALIDAR PRIMERO
        void EliminarSocio(int id);

    }
}

