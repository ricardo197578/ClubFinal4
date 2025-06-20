using System;
using System.Collections.Generic;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Services;

namespace ClubDeportivo.Services
{
    public interface IActividadService
    {
        List<Actividad> ObtenerActividadesDisponibles();
        List<Actividad> ObtenerActividadesParaNoSocios();
        Actividad ObtenerActividad(int id);
    }
}