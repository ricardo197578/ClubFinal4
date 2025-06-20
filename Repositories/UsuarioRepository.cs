using System;
using System.Data;
using System.Data.SQLite;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Helpers;

namespace ClubDeportivo.Repositories
{
    // Repositorio para manejar la persistencia de usuarios en la base de datos
    public class UsuarioRepository : IUsuarioRepository
    {
        // Helper para interacción con la base de datos SQLite
        private readonly DatabaseHelper _dbHelper;

        // Constructor que recibe el helper y asegura la creación de la tabla Usuarios
        public UsuarioRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase(); // Crear tabla si no existe
        }

        // Método privado para crear la tabla Usuarios si aún no existe
        private void InitializeDatabase()
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Usuarios (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        NombreUsuario TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT NOT NULL,
                        Rol TEXT NOT NULL,
                        FechaCreacion TEXT NOT NULL,
                        Activo INTEGER NOT NULL)";
            _dbHelper.ExecuteNonQuery(sql);
        }

        // Método para obtener un usuario activo por su nombre de usuario
        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            var sql = @"SELECT Id, NombreUsuario, PasswordHash, Rol, FechaCreacion, Activo 
                       FROM Usuarios WHERE NombreUsuario = @nombreUsuario AND Activo = 1";

            // Ejecuta consulta con parámetro nombreUsuario
            var dt = _dbHelper.ExecuteQuery(sql,
                new SQLiteParameter("@nombreUsuario", nombreUsuario));

            // Si no se encuentra, devuelve null
            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            // Mapea fila a objeto Usuario
            return new Usuario
            {
                Id = Convert.ToInt32(row["Id"]),
                NombreUsuario = row["NombreUsuario"].ToString(),
                PasswordHash = row["PasswordHash"].ToString(),
                Rol = row["Rol"].ToString(),
                FechaCreacion = DateTime.Parse(row["FechaCreacion"].ToString()),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        // Método para agregar un nuevo usuario a la base de datos
        public void Agregar(Usuario usuario)
        {
            var sql = @"INSERT INTO Usuarios 
                       (NombreUsuario, PasswordHash, Rol, FechaCreacion, Activo) 
                       VALUES 
                       (@nombreUsuario, @passwordHash, @rol, @fechaCreacion, @activo)";

            // Ejecuta inserción con parámetros para prevenir inyección SQL
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombreUsuario", usuario.NombreUsuario),

                // Nota: la línea comentada da error, por eso se usa directamente el hash ya calculado
                // new SQLiteParameter("@passwordHash", HashHelper.HashPassword(usuario.PasswordHash)), 

                new SQLiteParameter("@passwordHash", usuario.PasswordHash),

                new SQLiteParameter("@rol", usuario.Rol),
                new SQLiteParameter("@fechaCreacion", usuario.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss")),
                new SQLiteParameter("@activo", usuario.Activo ? 1 : 0)); // Boolean convertido a 1 o 0
        }
    }
}
