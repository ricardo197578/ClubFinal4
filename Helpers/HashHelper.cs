using System;
using System.Security.Cryptography;
using System.Text;

namespace ClubDeportivo.Helpers
{
    /// <summary>
    /// Clase est�tica que proporciona m�todos para el hashing y verificaci�n de contrase�as
    /// utilizando el algoritmo SHA-256.
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// Genera un hash SHA-256 de una contrase�a en texto plano
        /// </summary>
        /// <param name="password">Contrase�a en texto plano a hashear</param>
        /// <returns>Cadena hexadecimal que representa el hash de la contrase�a</returns>
        public static string HashPassword(string password)
        {
            // Usa SHA256 para crear el hash. El 'using' asegura la liberaci�n de recursos
            using (var sha256 = SHA256.Create())
            {
                // Convierte la contrase�a a bytes usando codificaci�n UTF-8
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convierte el array de bytes a una cadena hexadecimal
                // Elimina los guiones y convierte a min�sculas para consistencia
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Verifica si una contrase�a en texto plano coincide con un hash almacenado
        /// </summary>
        /// <param name="inputPassword">Contrase�a en texto plano a verificar</param>
        /// <param name="storedHash">Hash almacenado para comparaci�n</param>
        /// <returns>True si la contrase�a coincide, False en caso contrario</returns>
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            // Genera el hash de la contrase�a ingresada
            var hashOfInput = HashPassword(inputPassword);

            // Compara los hashes ignorando may�sculas/min�sculas
            return string.Equals(hashOfInput, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}