using System;

namespace ClubDeportivo.Models
{
    public class Usuario
    {
        private string _nombreUsuario;
        private string _passwordHash;
        private string _rol = "Admin";

        public int Id { get; set; }

        public string NombreUsuario
        {
            get { return _nombreUsuario; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de usuario es obligatorio");
                if (value.Length > 20)
                    throw new ArgumentException("El usuario no puede exceder 20 caracteres");
                _nombreUsuario = value;
            }
        }

        public string PasswordHash
        {
            get { return _passwordHash; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La contraseña es obligatoria");
                _passwordHash = value;
            }
        }

        public string Rol
        {
            get { return _rol; }
            set { _rol = value ?? "Admin"; }
        }

        public DateTime FechaCreacion { get; set; }

        public bool Activo { get; set; }

        public Usuario()
        {
            FechaCreacion = DateTime.Now;
            Activo = true;
        }

        // Método para validar todas las propiedades
        public bool Validar(out System.Collections.Generic.List<string> errores)
        {
            errores = new System.Collections.Generic.List<string>();

            try { var temp = NombreUsuario; }
            catch (Exception ex) { errores.Add(ex.Message); }

            try { var temp = PasswordHash; }
            catch (Exception ex) { errores.Add(ex.Message); }

            return errores.Count == 0;
        }
    }
}