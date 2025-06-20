using System;
using System.Data;
using System.Data.SQLite;
using ClubDeportivo.Models;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Helpers;

namespace ClubDeportivo.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public UsuarioRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase();
        }

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

        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            var sql = @"SELECT Id, NombreUsuario, PasswordHash, Rol, FechaCreacion, Activo 
                       FROM Usuarios WHERE NombreUsuario = @nombreUsuario AND Activo = 1";
            
            var dt = _dbHelper.ExecuteQuery(sql, 
                new SQLiteParameter("@nombreUsuario", nombreUsuario));

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
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

        public void Agregar(Usuario usuario)
        {
            var sql = @"INSERT INTO Usuarios 
                       (NombreUsuario, PasswordHash, Rol, FechaCreacion, Activo) 
                       VALUES 
                       (@nombreUsuario, @passwordHash, @rol, @fechaCreacion, @activo)";

            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombreUsuario", usuario.NombreUsuario),
                //new SQLiteParameter("@passwordHash", HashHelper.HashPassword(usuario.PasswordHash)),
                new SQLiteParameter("@passwordHash", usuario.PasswordHash),

                new SQLiteParameter("@rol", usuario.Rol),
                new SQLiteParameter("@fechaCreacion", usuario.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss")),
                new SQLiteParameter("@activo", usuario.Activo ? 1 : 0));
        }
    }
}