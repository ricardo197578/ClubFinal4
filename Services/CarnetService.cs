using System;
using System.Collections.Generic;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;

namespace ClubDeportivo.Services
{
    // Servicio que maneja la lógica relacionada con los carnets de socios
    public class CarnetService : ICarnetService
    {
        // Repositorio para acceder y manipular datos de carnets
        private readonly ICarnetRepository _carnetRepository;

        // Constructor que recibe el repositorio como dependencia
        public CarnetService(ICarnetRepository carnetRepository)
        {
            _carnetRepository = carnetRepository;
        }

        // Obtiene un carnet por su Id
        public Carnet GetCarnet(int id)
        {
            return _carnetRepository.GetById(id);
        }

        // Obtiene todos los carnets existentes
        public IEnumerable<Carnet> GetAllCarnets()
        {
            return _carnetRepository.GetAll();
        }

        // Crea un nuevo carnet
        public void CreateCarnet(Carnet carnet)
        {
            _carnetRepository.Add(carnet);
        }

        // Actualiza un carnet existente
        public void UpdateCarnet(Carnet carnet)
        {
            _carnetRepository.Update(carnet);
        }

        // Elimina un carnet por Id
        public void DeleteCarnet(int id)
        {
            _carnetRepository.Delete(id);
        }

        // Obtiene el carnet asociado a un socio dado su Id
        public Carnet GetCarnetBySocio(int socioId)
        {
            return _carnetRepository.GetBySocioId(socioId);
        }

        // Genera un carnet para un socio, si está apto físicamente
        public void GenerateCarnetForSocio(int socioId, bool aptoFisico)
        {
            if (!aptoFisico)
                throw new InvalidOperationException("No se puede generar carnet para socio no apto físicamente");

            // Verifica si el socio ya tiene un carnet
            var existingCarnet = _carnetRepository.GetBySocioId(socioId);
            if (existingCarnet != null)
            {
                // Actualiza la fecha de emisión y vencimiento, y marca apto físico
                existingCarnet.FechaEmision = DateTime.Now;
                existingCarnet.FechaVencimiento = DateTime.Now.AddYears(1);
                existingCarnet.AptoFisico = true;
                _carnetRepository.Update(existingCarnet);
            }
            else
            {
                // Crea un nuevo carnet asignando un nuevo número, fechas y aptitud física
                var newCarnet = new Carnet
                {
                    NroCarnet = _carnetRepository.GetNextCarnetNumber(),
                    FechaEmision = DateTime.Now,
                    FechaVencimiento = DateTime.Now.AddYears(1),
                    AptoFisico = true,
                    SocioId = socioId
                };
                _carnetRepository.Add(newCarnet);
            }
        }
    }
}
