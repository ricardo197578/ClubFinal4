// Importa funcionalidades b�sicas del sistema como tipos primitivos y excepciones
using System;

// Importa el modelo 'Usuario' definido en el proyecto
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para servicios de autenticaci�n de usuarios
    public interface IAuthService
    {
        // M�todo que verifica si las credenciales proporcionadas son v�lidas
        bool Autenticar(string nombreUsuario, string password);

        // M�todo que finaliza la sesi�n del usuario actualmente autenticado
        void CerrarSesion();

        // Propiedad que devuelve el usuario que ha iniciado sesi�n actualmente
        Usuario UsuarioActual { get; }

        // Propiedad booleana que indica si hay un usuario autenticado en el sistema
        bool EstaAutenticado { get; }
    }
}

