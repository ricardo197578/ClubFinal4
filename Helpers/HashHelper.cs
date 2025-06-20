using System;
using System.Security.Cryptography;
using System.Text;

namespace ClubDeportivo.Helpers
{
    /// <summary>
    /// Clase estática que proporciona métodos para el hashing y verificación de contraseñas
    /// utilizando el algoritmo SHA-256.
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// Genera un hash SHA-256 de una contraseña en texto plano
        /// </summary>
        /// <param name="password">Contraseña en texto plano a hashear</param>
        /// <returns>Cadena hexadecimal que representa el hash de la contraseña</returns>
        public static string HashPassword(string password)
        {
            // Usa SHA256 para crear el hash. El 'using' asegura la liberación de recursos
            using (var sha256 = SHA256.Create())
            {
                // Convierte la contraseña a bytes usando codificación UTF-8
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convierte el array de bytes a una cadena hexadecimal
                // Elimina los guiones y convierte a minúsculas para consistencia
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Verifica si una contraseña en texto plano coincide con un hash almacenado
        /// </summary>
        /// <param name="inputPassword">Contraseña en texto plano a verificar</param>
        /// <param name="storedHash">Hash almacenado para comparación</param>
        /// <returns>True si la contraseña coincide, False en caso contrario</returns>
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            // Genera el hash de la contraseña ingresada
            var hashOfInput = HashPassword(inputPassword);

            // Compara los hashes ignorando mayúsculas/minúsculas
            return string.Equals(hashOfInput, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}