// Importa el espacio de nombres System, que contiene tipos fundamentales y clases base
using System;
// Importa el espacio de nombres ClubDeportivo.Models, que contiene las clases de modelo
using ClubDeportivo.Models;

// Define el espacio de nombres ClubDeportivo.Interfaces que contiene las interfaces del proyecto
namespace ClubDeportivo.Interfaces
{
    // Declara la interfaz IUsuarioRepository que define las operaciones para el repositorio de usuarios
    public interface IUsuarioRepository
    {
        // Declara el método para obtener un usuario por su nombre de usuario
        // Recibe: nombreUsuario (string) - El nombre de usuario a buscar
        // Retorna: Usuario - El objeto Usuario encontrado o null si no existe
        Usuario ObtenerPorNombreUsuario(string nombreUsuario);

        // Declara el método para agregar un nuevo usuario al repositorio
        // Recibe: usuario (Usuario) - El objeto Usuario a agregar
        // No retorna ningún valor (void)
        void Agregar(Usuario usuario);
    }
}