using System;
using System.Collections.Generic;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;

namespace ClubDeportivo.Services
{
    // Servicio que maneja la lógica de negocio para NoSocios
    public class NoSocioService : INoSocioService
    {
        // Repositorio para acceder a datos de NoSocios
        private readonly INoSocioRepository _repository;

        // Constructor que recibe el repositorio como dependencia
        public NoSocioService(INoSocioRepository repository)
        {
            _repository = repository;
        }

        // Registra un nuevo NoSocio con nombre, apellido y DNI
        public void RegistrarNoSocio(string nombre, string apellido, string dni)
        {
            // Crea un objeto NoSocio con los datos recibidos
            var noSocio = new NoSocio { Nombre = nombre, Apellido = apellido, Dni = dni };
            // Lo agrega a la base de datos mediante el repositorio
            _repository.Agregar(noSocio);
        }

        // Obtiene la lista completa de NoSocios registrados
        public List<NoSocio> ObtenerNoSocios()
        {
            return _repository.ObtenerTodos();
        }

        // Busca un NoSocio por su DNI, validando que no sea vacío
        public NoSocio BuscarPorDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío");

            return _repository.BuscarPorDni(dni);
        }
    }
}
