using System;
using System.Collections.Generic;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Services;

namespace ClubDeportivo.Services

{
    public interface ICarnetService
    {
        Carnet GetCarnet(int id);
        IEnumerable<Carnet> GetAllCarnets();
        void CreateCarnet(Carnet carnet);
        void UpdateCarnet(Carnet carnet);
        void DeleteCarnet(int id);
        Carnet GetCarnetBySocio(int socioId);
        void GenerateCarnetForSocio(int socioId, bool aptoFisico);

       
    }
}