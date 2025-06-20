using System;
using System.Collections.Generic;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;

namespace ClubDeportivo.Services
{
    public class SocioService : ISocioService
    {
        // Campo privado readonly para el repositorio (solo asignable en el constructor)
        private readonly ISocioRepository _repository;

        // Constructor que recibe la dependencia ISocioRepository (Importante!!! == Inyección de Dependencias)
        // El servicio recibe el repositorio por constructor
        public SocioService(ISocioRepository repository)
        {
            _repository = repository;// Asigna el repositorio inyectado al campo privado
        }

        // Nueva versión acepta objeto Socio completo
        public void RegistrarSocio(Socio socio)
        {
            // Validación de objeto nulo 
            if (socio == null) throw new ArgumentNullException("socio");
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(socio.Nombre))
                throw new ArgumentException("El nombre es requerido");
            if (string.IsNullOrWhiteSpace(socio.Apellido))
                throw new ArgumentException("El apellido es requerido");
            if (string.IsNullOrWhiteSpace(socio.Dni))
                throw new ArgumentException("El DNI es requerido");
            if (ExisteDni(socio.Dni))
                throw new InvalidOperationException("Ya existe un socio con este DNI");

            // // Asignación de valores por defecto si no están establecidos
            if (socio.FechaInscripcion == DateTime.MinValue)
                socio.FechaInscripcion = DateTime.Now;
            if (socio.FechaVencimientoCuota == DateTime.MinValue)
                socio.FechaVencimientoCuota = DateTime.Now.AddMonths(1);
            if (socio.Tipo == 0) // Valor por defecto del enum
                socio.Tipo = TipoSocio.Standard;

            socio.EstadoActivo = true; // Siempre activo al registrarse

            _repository.Agregar(socio);// Persiste el socio
        }

        // Obtiene todos los socios (delega al repositorio)
        public List<Socio> ObtenerSocios()
        {
            return _repository.ObtenerTodos();
        }

        // Obtiene un socio por ID (delega al repositorio)
        public Socio GetSocio(int id)
        {
            return _repository.ObtenerPorId(id);
        }

        // Obtiene un socio por DNI (delega al repositorio)
        public Socio GetSocio(string dni)
        {
            return _repository.ObtenerPorDni(dni);
        }

        // Verifica si existe un socio con el DNI especificado
        //RECIBE UN STRING DNI COMO PARAMETRO
        //LLAMA AL METODO OBTENER POR DNI IMPLEMENTADO EN EL REPOSITORIO
        //COMPRARA SI EL RESULTDO ES DIFERENTE DE NULL
        //RETORNA TRUE SI ESXISTE (SI EXISTE NO ES NUL) Y FALSE SI NO EXISTE(ES NULL)
        public bool ExisteDni(string dni)
        {
            return _repository.ObtenerPorDni(dni) != null;
        }

        // Versión alternativa para obtener por ID (redundante con GetSocio por si da error revisar)
        public Socio ObtenerSocioPorId(int id)
        {
            return _repository.ObtenerPorId(id);
        }


        // Sobrecarga que permite especificar el tipo de socio
        /*blic void RegistrarSocio(string nombre, string apellido, string dni, TipoSocio tipo)
        {
            var socio = new Socio
            {
                Nombre = nombre,
                Apellido = apellido,
                Dni = dni,
                FechaInscripcion = DateTime.Now,
                FechaVencimientoCuota = DateTime.Now.AddMonths(1),
                EstadoActivo = true,
                Tipo = tipo // Usa el tipo especificado en lugar del por defecto Enum
            };
            this.RegistrarSocio(socio); // Reutiliza la versión completa
        }*/

        public void EliminarSocio(int id)
        {
            // Primero verificar si existe
            var socio = _repository.ObtenerPorId(id);
            if (socio == null)
                throw new ArgumentException("No se encontró el socio con el ID especificado");

            // Delega la eliminación al repositorio

            _repository.Eliminar(id);

        }
    }
}