using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    // Clase que implementa el repositorio de actividades (IActividadRepository)
    public class ActividadRepository : IActividadRepository
    {
        // Campo privado para el helper de base de datos
        private readonly DatabaseHelper _dbHelper;

        // Constructor que recibe el DatabaseHelper como dependencia
        public ActividadRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper; // Asigna el helper de base de datos
            InitializeDatabase(); // Inicializa la estructura de la base de datos
        }

        // Método privado para inicializar la estructura de la tabla en la base de datos
        private void InitializeDatabase()
        {
            // SQL para crear la tabla si no existe
            var sql = @"CREATE TABLE IF NOT EXISTS Actividades (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Descripcion TEXT,
                        Horario TEXT,
                        Precio REAL NOT NULL,
                        ExclusivaSocios INTEGER NOT NULL DEFAULT 0)";
            _dbHelper.ExecuteNonQuery(sql); // Ejecuta el comando SQL
        }

        // Implementación del método para agregar una nueva actividad
        public void Agregar(Actividad actividad)
        {
            // SQL para insertar una nueva actividad
            var sql = @"INSERT INTO Actividades 
                        (Nombre, Descripcion, Horario, Precio, ExclusivaSocios) 
                        VALUES (@nombre, @desc, @horario, @precio, @exclusiva)";

            // Ejecuta el comando SQL con parámetros
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombre", actividad.Nombre),
                new SQLiteParameter("@desc", actividad.Descripcion),
                new SQLiteParameter("@horario", actividad.Horario),
                new SQLiteParameter("@precio", actividad.Precio),
                new SQLiteParameter("@exclusiva", actividad.ExclusivaSocios ? 1 : 0));
        }

        // Implementación del método para obtener todas las actividades
        public List<Actividad> ObtenerTodas()
        {
            var actividades = new List<Actividad>(); // Lista para almacenar resultados
            var sql = "SELECT * FROM Actividades"; // SQL para consultar todas las actividades
            var dt = _dbHelper.ExecuteQuery(sql); // Ejecuta la consulta

            // Recorre cada fila del resultado y crea objetos Actividad
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
            return actividades; // Retorna la lista de actividades
        }

        // Implementación del método para obtener una actividad por su ID
        public Actividad ObtenerPorId(int id)
        {
            var sql = "SELECT * FROM Actividades WHERE Id = @id"; // SQL con parámetro
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@id", id)); // Ejecuta consulta

            if (dt.Rows.Count == 0) return null; // Si no hay resultados, retorna null

            var row = dt.Rows[0]; // Toma la primera fila
            return new Actividad // Crea y retorna un objeto Actividad
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