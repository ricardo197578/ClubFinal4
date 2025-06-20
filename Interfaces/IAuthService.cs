// Importa funcionalidades básicas del sistema como tipos primitivos y excepciones
using System;

// Importa el modelo 'Usuario' definido en el proyecto
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    // Define una interfaz para servicios de autenticación de usuarios
    public interface IAuthService
    {
        // Método que verifica si las credenciales proporcionadas son válidas
        bool Autenticar(string nombreUsuario, string password);

        // Método que finaliza la sesión del usuario actualmente autenticado
        void CerrarSesion();

        // Propiedad que devuelve el usuario que ha iniciado sesión actualmente
        Usuario UsuarioActual { get; }

        // Propiedad booleana que indica si hay un usuario autenticado en el sistema
        bool EstaAutenticado { get; }
    }
}

