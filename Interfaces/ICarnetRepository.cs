// Importa funcionalidades b�sicas del sistema, como tipos primitivos y excepciones
using System;

// Importa el espacio de nombres necesario para trabajar con colecciones gen�ricas
using System.Collections.Generic;

// Importa el modelo 'Carnet' definido en el proyecto
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para las operaciones que se pueden realizar sobre carnets
    public interface ICarnetRepository
    {
        // Devuelve un carnet seg�n su identificador �nico (ID)
        Carnet GetById(int id);

        // Devuelve una colecci�n con todos los carnets registrados
        IEnumerable<Carnet> GetAll();

        // Agrega un nuevo carnet al repositorio
        void Add(Carnet carnet);

        // Actualiza los datos de un carnet existente
        void Update(Carnet carnet);

        // Elimina un carnet a partir de su ID
        void Delete(int id);

        // Devuelve el carnet asociado a un socio espec�fico (por ID del socio)
        Carnet GetBySocioId(int socioId);

        // Devuelve el siguiente n�mero de carnet disponible, �til para numeraci�n secuencial
        int GetNextCarnetNumber();
    }
}
