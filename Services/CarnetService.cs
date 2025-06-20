using System;
using System.Collections.Generic;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;

namespace ClubDeportivo.Services
{
    public class CarnetService : ICarnetService
    {
        private readonly ICarnetRepository _carnetRepository;

        public CarnetService(ICarnetRepository carnetRepository)
        {
            _carnetRepository = carnetRepository;
        }

        public Carnet GetCarnet(int id)
        {
            return _carnetRepository.GetById(id);
        }

        public IEnumerable<Carnet> GetAllCarnets()
        {
            return _carnetRepository.GetAll();
        }

        public void CreateCarnet(Carnet carnet)
        {
            _carnetRepository.Add(carnet);
        }

        public void UpdateCarnet(Carnet carnet)
        {
            _carnetRepository.Update(carnet);
        }

        public void DeleteCarnet(int id)
        {
            _carnetRepository.Delete(id);
        }

        public Carnet GetCarnetBySocio(int socioId)
        {
            return _carnetRepository.GetBySocioId(socioId);
        }

        public void GenerateCarnetForSocio(int socioId, bool aptoFisico)
        {
            if (!aptoFisico)
                throw new InvalidOperationException("No se puede generar carnet para socio no apto físicamente");

            var existingCarnet = _carnetRepository.GetBySocioId(socioId);
            if (existingCarnet != null)
            {
                // Actualizar carnet existente
                existingCarnet.FechaEmision = DateTime.Now;
                existingCarnet.FechaVencimiento = DateTime.Now.AddYears(1);
                existingCarnet.AptoFisico = true;
                _carnetRepository.Update(existingCarnet);
            }
            else
            {
                // Crear nuevo carnet
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