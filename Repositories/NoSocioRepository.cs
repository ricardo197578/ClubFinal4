using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using System.Linq;

namespace ClubDeportivo.Repositories
{
    public class NoSocioRepository : INoSocioRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public NoSocioRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            var sql = @"CREATE TABLE IF NOT EXISTS NoSocios (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Apellido TEXT NOT NULL,
                        Dni TEXT NOT NULL,
                        FechaRegistro TEXT NOT NULL)";
            _dbHelper.ExecuteNonQuery(sql);
        }

        public void Agregar(NoSocio noSocio)
        {
            var sql = "INSERT INTO NoSocios (Nombre, Apellido,Dni, FechaRegistro) VALUES (@nombre, @apellido,@dni, @fecha)";
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombre", noSocio.Nombre),
                new SQLiteParameter("@apellido", noSocio.Apellido),
                new SQLiteParameter("@dni", noSocio.Dni),
                new SQLiteParameter("@fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        public List<NoSocio> ObtenerTodos()
        {
            var noSocios = new List<NoSocio>();
            var sql = "SELECT Id, Nombre, Apellido ,Dni ,FechaRegistro FROM NoSocios";
            var dt = _dbHelper.ExecuteQuery(sql);

            foreach (System.Data.DataRow row in dt.Rows)
            {
                noSocios.Add(new NoSocio
                {
                    Id = (int)(long)row["Id"],
                    Nombre = row["Nombre"].ToString(),
                    Apellido = row["Apellido"].ToString(),
                    Dni = row["Dni"].ToString(),
                    FechaRegistro = DateTime.Parse(row["FechaRegistro"].ToString())
                });
            }
            return noSocios;
        }

        public NoSocio ObtenerPorId(int id)
        {
            var sql = "SELECT Id, Nombre, Apellido,Dni, FechaRegistro FROM NoSocios WHERE Id = @id";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@id", id));

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new NoSocio
            {
                Id = (int)(long)row["Id"],
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Dni = row["Dni"].ToString(),
                FechaRegistro = DateTime.Parse(row["FechaRegistro"].ToString())
            };
        }

        public NoSocio BuscarPorDni(string dni)
        {
            // para buscar en base de datos
            var sql = "SELECT * FROM NoSocios WHERE Dni = @dni";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@dni", dni));

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new NoSocio
            {
                Id = Convert.ToInt32(row["Id"]),

                Apellido = row["Apellido"].ToString(),
                Dni = row["Dni"].ToString(),
                FechaRegistro = DateTime.Parse(row["FechaRegistro"].ToString())
                
            };
        }
    }
}