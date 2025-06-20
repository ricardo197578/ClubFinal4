using System;
using System.Collections.Generic;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;

namespace ClubDeportivo.Services

{
    public class NoSocioService : INoSocioService
    {
        private readonly INoSocioRepository _repository;

        public NoSocioService(INoSocioRepository repository)
        {
            _repository = repository;
        }

        public void RegistrarNoSocio(string nombre, string apellido, string dni)
        {
            var noSocio = new NoSocio { Nombre = nombre, Apellido = apellido, Dni = dni};
            _repository.Agregar(noSocio);
        }

        public List<NoSocio> ObtenerNoSocios()
        {
            return _repository.ObtenerTodos();
        }

        
        public NoSocio BuscarPorDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío");

            return _repository.BuscarPorDni(dni); 
        }
    }
}