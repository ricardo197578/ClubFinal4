using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    public class ActividadRepository : IActividadRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public ActividadRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Actividades (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Descripcion TEXT,
                        Horario TEXT,
                        Precio REAL NOT NULL,
                        ExclusivaSocios INTEGER NOT NULL DEFAULT 0)";
            _dbHelper.ExecuteNonQuery(sql);
        }

        public void Agregar(Actividad actividad)
        {
            var sql = @"INSERT INTO Actividades 
                        (Nombre, Descripcion, Horario, Precio, ExclusivaSocios) 
                        VALUES (@nombre, @desc, @horario, @precio, @exclusiva)";
            
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombre", actividad.Nombre),
                new SQLiteParameter("@desc", actividad.Descripcion),
                new SQLiteParameter("@horario", actividad.Horario),
                new SQLiteParameter("@precio", actividad.Precio),
                new SQLiteParameter("@exclusiva", actividad.ExclusivaSocios ? 1 : 0));
        }

        public List<Actividad> ObtenerTodas()
        {
            var actividades = new List<Actividad>();
            var sql = "SELECT * FROM Actividades";
            var dt = _dbHelper.ExecuteQuery(sql);

            foreach (System.Data.DataRow row in dt.Rows)
            {
                actividades.Add(new Actividad
                {
                    Id = (int)(long)row["Id"],
                    Nombre = row["Nombre"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    Horario = row["Horario"].ToString(),
                    Precio = Convert.ToDecimal(row["Precio"]),
                    ExclusivaSocios = (int)(long)row["ExclusivaSocios"] == 1
                });
            }
            return actividades;
        }

        public Actividad ObtenerPorId(int id)
        {
            var sql = "SELECT * FROM Actividades WHERE Id = @id";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@id", id));

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new Actividad
            {
                Id = (int)(long)row["Id"],
                Nombre = row["Nombre"].ToString(),
                Descripcion = row["Descripcion"].ToString(),
                Horario = row["Horario"].ToString(),
                Precio = Convert.ToDecimal(row["Precio"]),
                ExclusivaSocios = (int)(long)row["ExclusivaSocios"] == 1
            };
        }
    }
}