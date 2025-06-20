using System;
using System.Collections.Generic;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces

{
    public interface ICarnetRepository
    {
        Carnet GetById(int id);
        IEnumerable<Carnet> GetAll();
        void Add(Carnet carnet);
        void Update(Carnet carnet);
        void Delete(int id);
        Carnet GetBySocioId(int socioId);
        int GetNextCarnetNumber();

        
    }
}