// Importa funcionalidades básicas del sistema como tipos primitivos y excepciones
using System;

// Importa el espacio de nombres necesario para trabajar con colecciones genéricas
using System.Collections.Generic;

// Importa el modelo 'Carnet' utilizado por esta interfaz
using ClubDeportivo.Models;

// Importa interfaces del proyecto, aunque no se utilizan directamente en este archivo
using ClubDeportivo.Interfaces;

// Importa servicios del proyecto, aunque este archivo define una nueva interfaz de servicio
using ClubDeportivo.Services;

namespace ClubDeportivo.Services
{
    // Define una interfaz para los servicios relacionados con la gestión de carnets
    public interface ICarnetService
    {
        // Obtiene un carnet a partir de su identificador único
        Carnet GetCarnet(int id);

        // Devuelve todos los carnets registrados en el sistema
        IEnumerable<Carnet> GetAllCarnets();

        // Crea un nuevo carnet en el sistema
        void CreateCarnet(Carnet carnet);

        // Actualiza los datos de un carnet existente
        void UpdateCarnet(Carnet carnet);

        // Elimina un carnet utilizando su identificador único
        void DeleteCarnet(int id);

        // Obtiene el carnet asociado a un socio específico
        Carnet GetCarnetBySocio(int socioId);

        // Genera un carnet para un socio determinado, considerando si tiene apto físico o no
        void GenerateCarnetForSocio(int socioId, bool aptoFisico);
    }
}
