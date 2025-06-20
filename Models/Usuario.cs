// Archivo: Usuario.cs
using System;

namespace ClubDeportivo.Models
{
    // Clase que representa un usuario del sistema
    public class Usuario
    {
        // Campos privados
        private string _nombreUsuario;
        private string _passwordHash;
        private string _rol = "Admin";

        public int Id { get; set; } // Identificador único

        // Propiedad con validación para el nombre de usuario
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

        // Propiedad con validación para la contraseña hasheada
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

        // Propiedad para el rol con valor por defecto "Admin"
        public string Rol
        {
            get { return _rol; }
            set { _rol = value ?? "Admin"; }
        }

        public DateTime FechaCreacion { get; set; } // Fecha de creación
        public bool Activo { get; set; } // Estado activo/inactivo

        // Constructor que inicializa valores por defecto
        public Usuario()
        {
            FechaCreacion = DateTime.Now; // Fecha actual
            Activo = true; // Por defecto activo
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