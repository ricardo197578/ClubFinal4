// Importa funcionalidades básicas del sistema, como tipos primitivos y excepciones
using System;

// Importa el espacio de nombres necesario para trabajar con listas genéricas
using System.Collections.Generic;

// Importa el modelo 'Actividad' definido en el proyecto
using ClubDeportivo.Models;

// Importa interfaces del proyecto, aunque en este archivo no se usan directamente
using ClubDeportivo.Interfaces;

// Importa servicios del proyecto, aunque este archivo define uno nuevo
using ClubDeportivo.Services;

namespace ClubDeportivo.Services
{
    // Define una interfaz que representa los servicios relacionados con las actividades
    public interface IActividadService
    {
        // Devuelve una lista de todas las actividades disponibles para cualquier tipo de usuario
        List<Actividad> ObtenerActividadesDisponibles();

        // Devuelve una lista de actividades especialmente habilitadas para personas que no son socias
        List<Actividad> ObtenerActividadesParaNoSocios();

        // Devuelve una actividad específica según su identificador único (ID)
        Actividad ObtenerActividad(int id);
    }
}
