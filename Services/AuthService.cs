using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using ClubDeportivo.Helpers;

namespace ClubDeportivo.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private Usuario _usuarioActual;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Usuario UsuarioActual
        {
            get { return _usuarioActual; }
        }

        public bool EstaAutenticado
        {
            get { return _usuarioActual != null; }
        }

        public bool Autenticar(string nombreUsuario, string password)
        {
            var usuario = _usuarioRepository.ObtenerPorNombreUsuario(nombreUsuario);
            if (usuario == null)
                return false;

            if (!HashHelper.VerifyPassword(password, usuario.PasswordHash))
                return false;

            _usuarioActual = usuario;
            return true;
        }

        public void CerrarSesion()
        {
            _usuarioActual = null;
        }
    }
}
