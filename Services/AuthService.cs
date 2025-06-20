using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using ClubDeportivo.Helpers;

namespace ClubDeportivo.Services
{
    // Servicio para manejar autenticaci�n de usuarios
    public class AuthService : IAuthService
    {
        // Repositorio para acceder a datos de usuarios
        private readonly IUsuarioRepository _usuarioRepository;

        // Usuario que est� actualmente autenticado
        private Usuario _usuarioActual;

        // Constructor que recibe el repositorio de usuarios
        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Propiedad p�blica para obtener el usuario actualmente autenticado
        public Usuario UsuarioActual
        {
            get { return _usuarioActual; }
        }

        // Propiedad que indica si hay un usuario autenticado (true si _usuarioActual no es null)
        public bool EstaAutenticado
        {
            get { return _usuarioActual != null; }
        }

        // M�todo que intenta autenticar al usuario con nombre y contrase�a
        public bool Autenticar(string nombreUsuario, string password)
        {
            // Obtiene usuario por nombre de usuario
            var usuario = _usuarioRepository.ObtenerPorNombreUsuario(nombreUsuario);
            if (usuario == null)
                return false; // No existe usuario con ese nombre

            // Verifica si la contrase�a ingresada coincide con el hash almacenado
            if (!HashHelper.VerifyPassword(password, usuario.PasswordHash))
                return false; // Contrase�a incorrecta

            // Autenticaci�n exitosa: guarda el usuario actual
            _usuarioActual = usuario;
            return true;
        }

        // M�todo para cerrar la sesi�n, limpiando el usuario autenticado
        public void CerrarSesion()
        {
            _usuarioActual = null;
        }
    }
}
