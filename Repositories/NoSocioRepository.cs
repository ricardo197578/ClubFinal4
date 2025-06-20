using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;
using System.Linq;

namespace ClubDeportivo.Repositories
{
    // Repositorio para manejar la persistencia de NoSocios en la base de datos
    public class NoSocioRepository : INoSocioRepository
    {
        // Instancia de helper para acceder a la base de datos SQLite
        private readonly DatabaseHelper _dbHelper;

        // Constructor que recibe el helper y asegura la creación de la tabla
        public NoSocioRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase(); // Crear tabla si no existe
        }

        // Método privado para crear la tabla NoSocios si aún no existe
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

        // Método para agregar un nuevo NoSocio a la base de datos
        public void Agregar(NoSocio noSocio)
        {
            var sql = "INSERT INTO NoSocios (Nombre, Apellido,Dni, FechaRegistro) VALUES (@nombre, @apellido,@dni, @fecha)";
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombre", noSocio.Nombre),     // Parámetro nombre
                new SQLiteParameter("@apellido", noSocio.Apellido), // Parámetro apellido
                new SQLiteParameter("@dni", noSocio.Dni),           // Parámetro dni
                new SQLiteParameter("@fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))); // Fecha actual
        }

        // Método para obtener todos los NoSocios registrados
        public List<NoSocio> ObtenerTodos()
        {
            var noSocios = new List<NoSocio>();
            var sql = "SELECT Id, Nombre, Apellido ,Dni ,FechaRegistro FROM NoSocios";

            // Ejecuta consulta y obtiene un DataTable con resultados
            var dt = _dbHelper.ExecuteQuery(sql);

            // Itera cada fila del resultado para mapear a objetos NoSocio
            foreach (System.Data.DataRow row in dt.Rows)
            {
                noSocios.Add(new NoSocio
                {
                    Id = (int)(long)row["Id"], // Conversión de tipo long a int
                    Nombre = row["Nombre"].ToString(),
                    Apellido = row["Apellido"].ToString(),
                    Dni = row["Dni"].ToString(),
                    FechaRegistro = DateTime.Parse(row["FechaRegistro"].ToString()) // Parse de fecha
                });
            }
            return noSocios; // Devuelve la lista completa
        }

        // Método para obtener un NoSocio por su Id
        public NoSocio ObtenerPorId(int id)
        {
            var sql = "SELECT Id, Nombre, Apellido,Dni, FechaRegistro FROM NoSocios WHERE Id = @id";

            // Ejecuta consulta con parámetro id
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@id", id));

            // Si no encuentra resultados, devuelve null
            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            // Mapea la fila a un objeto NoSocio y lo devuelve
            return new NoSocio
            {
                Id = (int)(long)row["Id"],
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Dni = row["Dni"].ToString(),
                FechaRegistro = DateTime.Parse(row["FechaRegistro"].ToString())
            };
        }

        // Método para buscar un NoSocio por su DNI
        public NoSocio BuscarPorDni(string dni)
        {
            // Consulta para buscar por DNI usando parámetro
            var sql = "SELECT * FROM NoSocios WHERE Dni = @dni";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@dni", dni));

            // Si no encuentra resultados, devuelve null
            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            // Mapea la fila a un objeto NoSocio
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
