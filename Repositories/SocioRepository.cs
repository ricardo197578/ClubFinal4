using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    /// <summary>
    /// Implementación concreta de ISocioRepository que maneja el almacenamiento
    /// y recuperación de datos de socios en una base de datos SQLite.
    /// </summary>
    public class SocioRepository : ISocioRepository
    {
        // Campo privado readonly para el helper de base de datos
        // - readonly: Solo puede ser asignado en el constructor
        // - Privado: Solo accesible dentro de esta clase
        private readonly DatabaseHelper _dbHelper;

        /// <summary>
        /// Constructor que recibe las dependencias necesarias
        /// </summary>
        /// <param name="dbHelper">Instancia de DatabaseHelper para operaciones de BD</param>
        public SocioRepository(DatabaseHelper dbHelper)
        {
            // Validación básica del parámetro
            if (dbHelper == null)
                throw new ArgumentNullException("dbHelper", "El helper de base de datos no puede ser nulo");

            _dbHelper = dbHelper;

            // Inicializa la estructura de la base de datos
            InitializeDatabase();
        }

        /// <summary>
        /// Crea la tabla Socios si no existe en la base de datos
        /// </summary>
        private void InitializeDatabase()
        {
            // SQL para crear la tabla con todos los campos necesarios
            var sql = @"CREATE TABLE IF NOT EXISTS Socios (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Dni TEXT NOT NULL UNIQUE,
                        Apellido TEXT NOT NULL,
                        FechaInscripcion TEXT NOT NULL,
                        FechaVencimientoCuota TEXT NOT NULL,
                        EstadoActivo INTEGER NOT NULL,
                        Tipo INTEGER NOT NULL)";

            // Ejecuta el comando SQL
            _dbHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// Agrega un nuevo socio a la base de datos
        /// </summary>
        /// <param name="socio">Objeto Socio con los datos a insertar</param>
        public void Agregar(Socio socio)
        {
            // Validación del parámetro
            if (socio == null)
                throw new ArgumentNullException("socio", "El socio no puede ser nulo");

            // Consulta SQL parametrizada para prevenir inyección SQL
            var sql = @"INSERT INTO Socios 
                        (Nombre, Apellido, Dni, FechaInscripcion, FechaVencimientoCuota, EstadoActivo, Tipo) 
                        VALUES 
                        (@nombre, @apellido, @dni, @fechaInscripcion, @fechaVencimiento, @estadoActivo, @tipo)";

            // Ejecuta la consulta con todos los parámetros necesarios
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombre", socio.Nombre),
                new SQLiteParameter("@apellido", socio.Apellido),
                new SQLiteParameter("@dni", socio.Dni),
                // Conversión de DateTime a string con formato estandarizado
                new SQLiteParameter("@fechaInscripcion", socio.FechaInscripcion.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@fechaVencimiento", socio.FechaVencimientoCuota.ToString("yyyy-MM-dd")),
                // Conversión de bool a integer (1/0)
                new SQLiteParameter("@estadoActivo", socio.EstadoActivo ? 1 : 0),
                // Conversión de enum a su valor entero
                new SQLiteParameter("@tipo", (int)socio.Tipo));
        }

        /// <summary>
        /// Obtiene todos los socios registrados en la base de datos
        /// </summary>
        /// <returns>Lista de objetos Socio</returns>
        public List<Socio> ObtenerTodos()
        {
            // Lista que contendrá los resultados
            var socios = new List<Socio>();

            // Consulta SQL para obtener todos los registros
            var sql = @"SELECT Id, Nombre, Apellido, Dni, FechaInscripcion, FechaVencimientoCuota, EstadoActivo, Tipo 
                        FROM Socios";

            // Ejecuta la consulta y obtiene los resultados en un DataTable
            var dt = _dbHelper.ExecuteQuery(sql);

            // Recorre cada fila del resultado y crea objetos Socio
            foreach (System.Data.DataRow row in dt.Rows)
            {
                socios.Add(new Socio
                {
                    // Conversión segura de tipos desde SQLite a C#
                    Id = (int)(long)row["Id"], // SQLite devuelve INTEGER como long
                    Nombre = row["Nombre"].ToString(),
                    Apellido = row["Apellido"].ToString(),
                    Dni = row["Dni"].ToString(),
                    // Parseo de fechas desde string
                    FechaInscripcion = DateTime.Parse(row["FechaInscripcion"].ToString()),
                    FechaVencimientoCuota = DateTime.Parse(row["FechaVencimientoCuota"].ToString()),
                    // Conversión de INTEGER (1/0) a bool
                    EstadoActivo = Convert.ToBoolean(row["EstadoActivo"]),
                    // Conversión de INTEGER a enum
                    Tipo = (TipoSocio)Convert.ToInt32(row["Tipo"])
                });
            }

            return socios;
        }

        /// <summary>
        /// Obtiene un socio por su número de DNI
        /// </summary>
        /// <param name="dni">Número de documento a buscar</param>
        /// <returns>Objeto Socio o null si no se encuentra</returns>
        public Socio ObtenerPorDni(string dni)
        {
            // Validación del parámetro
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío", "dni");

            // Consulta SQL parametrizada para evitar inyeccion SQL
            var sql = @"SELECT Id, Nombre, Apellido, Dni, FechaInscripcion, FechaVencimientoCuota, EstadoActivo, Tipo 
                        FROM Socios WHERE Dni = @dni";

            // Ejecuta la consulta usando el helper de la base de datos con el parámetro DNI
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@dni", dni));

            // Si no hay resultados, devuelve null (manejo de resultado)
            if (dt.Rows.Count == 0)
                return null;

            // Obtiene la primera fila (debería ser única por la restricción UNIQUE)
            var row = dt.Rows[0];

            // Crea y devuelve el objeto Socio
            return new Socio
            {
                Id = (int)(long)row["Id"],
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Dni = row["Dni"].ToString(),
                FechaInscripcion = DateTime.Parse(row["FechaInscripcion"].ToString()),
                FechaVencimientoCuota = DateTime.Parse(row["FechaVencimientoCuota"].ToString()),
                EstadoActivo = Convert.ToBoolean(row["EstadoActivo"]),
                Tipo = (TipoSocio)Convert.ToInt32(row["Tipo"])
            };
        }

        /// <summary>
        /// Obtiene un socio por su ID único
        /// </summary>
        /// <param name="id">Identificador del socio</param>
        /// <returns>Objeto Socio o null si no se encuentra</returns>
        public Socio ObtenerPorId(int id)
        {
            try
            {
                // Consulta SQL parametrizada
                var sql = @"SELECT Id, Nombre, Apellido, Dni, FechaInscripcion, FechaVencimientoCuota, EstadoActivo, Tipo 
                            FROM Socios WHERE Id = @id";

                // Ejecuta la consulta
                var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@id", id));

                // Verifica si hay resultados
                if (dt == null || dt.Rows.Count == 0)
                {
                    // Registro para depuración (debería usarse un sistema de logging real)
                    Console.WriteLine(string.Format("No se encontraron registros para el ID:{0}", id));
                    return null;
                }

                // Obtiene la primera fila
                var row = dt.Rows[0];

                // Crea el objeto Socio con manejo de posibles valores null
                return new Socio
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"] != null ? row["Nombre"].ToString() : string.Empty,
                    Apellido = row["Apellido"] != null ? row["Apellido"].ToString() : string.Empty,
                    Dni = row["Dni"] != null ? row["Dni"].ToString() : string.Empty,
                    FechaInscripcion = row["FechaInscripcion"] != null ?
                        DateTime.Parse(row["FechaInscripcion"].ToString()) : DateTime.MinValue,
                    FechaVencimientoCuota = row["FechaVencimientoCuota"] != null ?
                        DateTime.Parse(row["FechaVencimientoCuota"].ToString()) : DateTime.MinValue,
                    EstadoActivo = row["EstadoActivo"] != null ?
                        Convert.ToBoolean(row["EstadoActivo"]) : false,
                    Tipo = row["Tipo"] != null ?
                        (TipoSocio)Convert.ToInt32(row["Tipo"]) : TipoSocio.Standard
                };
            }
            catch (Exception ex)
            {
                // Registro del error (debería usarse un sistema de logging real)
                Console.WriteLine(string.Format("Error en ObtenerPorId: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Actualiza los datos de un socio existente
        /// </summary>
        /// <param name="socio">Objeto Socio con los datos actualizados</param>
        public void Actualizar(Socio socio)
        {
            // Validación del parámetro
            if (socio == null)
                throw new ArgumentNullException("socio", "El socio no puede ser nulo");

            // Consulta SQL parametrizada para actualización
            var sql = @"UPDATE Socios SET 
                        Nombre = @nombre,
                        Apellido = @apellido,
                        Dni = @dni,
                        FechaInscripcion = @fechaInscripcion,
                        FechaVencimientoCuota = @fechaVencimiento,
                        EstadoActivo = @estadoActivo,
                        Tipo = @tipo
                        WHERE Id = @id";

            // Ejecuta la consulta con todos los parámetros
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombre", socio.Nombre),
                new SQLiteParameter("@apellido", socio.Apellido),
                new SQLiteParameter("@dni", socio.Dni),
                new SQLiteParameter("@fechaInscripcion", socio.FechaInscripcion.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@fechaVencimiento", socio.FechaVencimientoCuota.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@estadoActivo", socio.EstadoActivo ? 1 : 0),
                new SQLiteParameter("@tipo", (int)socio.Tipo),
                new SQLiteParameter("@id", socio.Id));
        }

        /// <summary>
        /// Elimina un socio de la base de datos por su ID
        /// </summary>
        /// <param name="id">Identificador del socio a eliminar</param>
        public void Eliminar(int id)
        {
            // Consulta SQL parametrizada para eliminación
            var sql = "DELETE FROM Socios WHERE Id = @id";

            // Ejecuta la consulta
            _dbHelper.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
        }
    }
}