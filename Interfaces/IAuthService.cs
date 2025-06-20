// En Interfaces/IAuthService.cs
using System;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    public interface IAuthService
    {
        bool Autenticar(string nombreUsuario, string password);
        void CerrarSesion();
        Usuario UsuarioActual { get; }
        bool EstaAutenticado { get; }
    }
}

