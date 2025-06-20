using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using ClubDeportivo.Helpers;

namespace ClubDeportivo.Services
{
    // Servicio para manejar autenticación de usuarios
    public class AuthService : IAuthService
    {
        // Repositorio para acceder a datos de usuarios
        private readonly IUsuarioRepository _usuarioRepository;

        // Usuario que está actualmente autenticado
        private Usuario _usuarioActual;

        // Constructor que recibe el repositorio de usuarios
        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Propiedad pública para obtener el usuario actualmente autenticado
        public Usuario UsuarioActual
        {
            get { return _usuarioActual; }
        }

        // Propiedad que indica si hay un usuario autenticado (true si _usuarioActual no es null)
        public bool EstaAutenticado
        {
            get { return _usuarioActual != null; }
        }

        // Método que intenta autenticar al usuario con nombre y contraseña
        public bool Autenticar(string nombreUsuario, string password)
        {
            // Obtiene usuario por nombre de usuario
            var usuario = _usuarioRepository.ObtenerPorNombreUsuario(nombreUsuario);
            if (usuario == null)
                return false; // No existe usuario con ese nombre

            // Verifica si la contraseña ingresada coincide con el hash almacenado
            if (!HashHelper.VerifyPassword(password, usuario.PasswordHash))
                return false; // Contraseña incorrecta

            // Autenticación exitosa: guarda el usuario actual
            _usuarioActual = usuario;
            return true;
        }

        // Método para cerrar la sesión, limpiando el usuario autenticado
        public void CerrarSesion()
        {
            _usuarioActual = null;
        }
    }
}
