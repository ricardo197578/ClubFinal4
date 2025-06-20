using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    public class SocioRepository : ISocioRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public SocioRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Socios (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Dni TEXT NOT NULL,
                        Apellido TEXT NOT NULL,
                        FechaInscripcion TEXT NOT NULL,
                        FechaVencimientoCuota TEXT NOT NULL,
                        EstadoActivo INTEGER NOT NULL,
                        Tipo INTEGER NOT NULL)";
            _dbHelper.ExecuteNonQuery(sql);
        }

        public void Agregar(Socio socio)
        {
            var sql = @"INSERT INTO Socios 
                        (Nombre, Apellido, Dni, FechaInscripcion, FechaVencimientoCuota, EstadoActivo, Tipo) 
                        VALUES 
                        (@nombre, @apellido, @dni, @fechaInscripcion, @fechaVencimiento, @estadoActivo, @tipo)";

            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nombre", socio.Nombre),
                new SQLiteParameter("@apellido", socio.Apellido),
                new SQLiteParameter("@dni", socio.Dni),
                new SQLiteParameter("@fechaInscripcion", socio.FechaInscripcion.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@fechaVencimiento", socio.FechaVencimientoCuota.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@estadoActivo", socio.EstadoActivo ? 1 : 0),
                new SQLiteParameter("@tipo", (int)socio.Tipo));
        }

        public List<Socio> ObtenerTodos()
        {
            var socios = new List<Socio>();
            var sql = @"SELECT Id, Nombre, Apellido, Dni, FechaInscripcion, FechaVencimientoCuota, EstadoActivo, Tipo 
                        FROM Socios";
            var dt = _dbHelper.ExecuteQuery(sql);

            foreach (System.Data.DataRow row in dt.Rows)
            {
                socios.Add(new Socio
                {
                    Id = (int)(long)row["Id"],
                    Nombre = row["Nombre"].ToString(),
                    Apellido = row["Apellido"].ToString(),
                    Dni = row["Dni"].ToString(),
                    FechaInscripcion = DateTime.Parse(row["FechaInscripcion"].ToString()),
                    FechaVencimientoCuota = DateTime.Parse(row["FechaVencimientoCuota"].ToString()),
                    EstadoActivo = Convert.ToBoolean(row["EstadoActivo"]),
                    Tipo = (TipoSocio)Convert.ToInt32(row["Tipo"])
                });
            }
            return socios;
        }

        public Socio ObtenerPorDni(string dni)
        {
            // Consulta SQL para buscar un socio por DNI
            var sql = @"SELECT Id, Nombre, Apellido, Dni, FechaInscripcion, FechaVencimientoCuota, EstadoActivo, Tipo 
                        FROM Socios WHERE Dni = @dni";

            // Ejecuta la consulta con el parámetro @dni
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@dni", dni));

            // Si no hay resultados, devuelve null (no existe)
            if (dt.Rows.Count == 0)
                return null;

            // Si existe, devuelve el socio encontrado
            var row = dt.Rows[0];
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

        public Socio ObtenerPorId(int id)
        {
            try
            {
                var sql = @"SELECT Id, Nombre, Apellido, Dni, FechaInscripcion, FechaVencimientoCuota, EstadoActivo, Tipo 
                            FROM Socios WHERE Id = @id";
                var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@id", id));

                if (dt == null || dt.Rows.Count == 0)
                {
                    Console.WriteLine(string.Format("No se encontraron registros para el ID:{0}", id));
                    return null;
                }

                var row = dt.Rows[0];
                return new Socio
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"] != null ? row["Nombre"].ToString() : string.Empty,
                    Apellido = row["Apellido"] != null ? row["Apellido"].ToString() : string.Empty,
                    Dni = row["Dni"] != null ? row["Dni"].ToString() : string.Empty,
                    FechaInscripcion = row["FechaInscripcion"] != null ? DateTime.Parse(row["FechaInscripcion"].ToString()) : DateTime.MinValue,
                    FechaVencimientoCuota = row["FechaVencimientoCuota"] != null ? DateTime.Parse(row["FechaVencimientoCuota"].ToString()) : DateTime.MinValue,
                    EstadoActivo = row["EstadoActivo"] != null ? Convert.ToBoolean(row["EstadoActivo"]) : false,
                    Tipo = row["Tipo"] != null ? (TipoSocio)Convert.ToInt32(row["Tipo"]) : TipoSocio.Standard
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error en ObtenerPorId:{0}", ex.Message));
                return null;
            }
        }

        public void Actualizar(Socio socio)
        {
            var sql = @"UPDATE Socios SET 
                        Nombre = @nombre,
                        Apellido = @apellido,
                        Dni = @dni,
                        FechaInscripcion = @fechaInscripcion,
                        FechaVencimientoCuota = @fechaVencimiento,
                        EstadoActivo = @estadoActivo,
                        Tipo = @tipo
                        WHERE Id = @id";

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
        //por las dudas
        public void Eliminar(int id)
        {
            var sql = "DELETE FROM Socios WHERE Id = @id";
            _dbHelper.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
        }
    }
}
       