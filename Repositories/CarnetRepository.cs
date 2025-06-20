using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    // Implementación del repositorio para la gestión de carnets (ICarnetRepository)
    public class CarnetRepository : ICarnetRepository
    {
        // Helper para la conexión y operaciones con la base de datos
        private readonly DatabaseHelper _dbHelper;

        // Constructor que recibe el DatabaseHelper como dependencia
        public CarnetRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper; // Asigna el helper de base de datos
            InitializeDatabase(); // Inicializa la estructura de la tabla
        }

        // Método para crear la tabla de carnets si no existe
        private void InitializeDatabase()
        {
            // SQL para crear la tabla con sus campos y relaciones
            var sql = @"CREATE TABLE IF NOT EXISTS Carnets (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        NroCarnet INTEGER NOT NULL,
                        FechaEmision TEXT NOT NULL,
                        FechaVencimiento TEXT NOT NULL,
                        AptoFisico INTEGER NOT NULL,
                        SocioId INTEGER NOT NULL,
                        FOREIGN KEY(SocioId) REFERENCES Socios(Id))";
            _dbHelper.ExecuteNonQuery(sql); // Ejecuta el comando SQL
        }

        // Obtiene un carnet por su ID
        public Carnet GetById(int id)
        {
            // Consulta SQL para buscar por ID
            var sql = "SELECT Id, NroCarnet, FechaEmision, FechaVencimiento, AptoFisico, SocioId FROM Carnets WHERE Id = @id";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@id", id));

            if (dt.Rows.Count == 0) return null; // Retorna null si no encuentra resultados

            var row = dt.Rows[0]; // Toma la primera fila
            return new Carnet // Crea y retorna el objeto Carnet
            {
                Id = (int)(long)row["Id"],
                NroCarnet = (int)(long)row["NroCarnet"],
                FechaEmision = DateTime.Parse(row["FechaEmision"].ToString()),
                FechaVencimiento = DateTime.Parse(row["FechaVencimiento"].ToString()),
                AptoFisico = Convert.ToBoolean((long)row["AptoFisico"]),
                SocioId = (int)(long)row["SocioId"]
            };
        }

        // Obtiene todos los carnets existentes
        public IEnumerable<Carnet> GetAll()
        {
            var carnets = new List<Carnet>(); // Lista para almacenar resultados
            // Consulta SQL para obtener todos los carnets
            var sql = "SELECT Id, NroCarnet, FechaEmision, FechaVencimiento, AptoFisico, SocioId FROM Carnets";
            var dt = _dbHelper.ExecuteQuery(sql); // Ejecuta la consulta

            // Convierte cada fila en un objeto Carnet
            foreach (System.Data.DataRow row in dt.Rows)
            {
                carnets.Add(new Carnet
                {
                    Id = (int)(long)row["Id"],
                    NroCarnet = (int)(long)row["NroCarnet"],
                    FechaEmision = DateTime.Parse(row["FechaEmision"].ToString()),
                    FechaVencimiento = DateTime.Parse(row["FechaVencimiento"].ToString()),
                    AptoFisico = Convert.ToBoolean((long)row["AptoFisico"]),
                    SocioId = (int)(long)row["SocioId"]
                });
            }
            return carnets; // Retorna la lista completa
        }

        // Agrega un nuevo carnet a la base de datos
        public void Add(Carnet carnet)
        {
            // SQL para insertar un nuevo registro
            var sql = @"INSERT INTO Carnets (NroCarnet, FechaEmision, FechaVencimiento, AptoFisico, SocioId) 
                        VALUES (@nroCarnet, @fechaEmision, @fechaVencimiento, @aptoFisico, @socioId)";
            _dbHelper.ExecuteNonQuery(sql, // Ejecuta con parámetros
                new SQLiteParameter("@nroCarnet", carnet.NroCarnet),
                new SQLiteParameter("@fechaEmision", carnet.FechaEmision.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@fechaVencimiento", carnet.FechaVencimiento.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@aptoFisico", carnet.AptoFisico ? 1 : 0),
                new SQLiteParameter("@socioId", carnet.SocioId));
        }

        // Actualiza un carnet existente
        public void Update(Carnet carnet)
        {
            // SQL para actualizar todos los campos del carnet
            var sql = @"UPDATE Carnets SET 
                        NroCarnet = @nroCarnet,
                        FechaEmision = @fechaEmision,
                        FechaVencimiento = @fechaVencimiento,
                        AptoFisico = @aptoFisico,
                        SocioId = @socioId
                        WHERE Id = @id";
            _dbHelper.ExecuteNonQuery(sql, // Ejecuta con parámetros
                new SQLiteParameter("@nroCarnet", carnet.NroCarnet),
                new SQLiteParameter("@fechaEmision", carnet.FechaEmision.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@fechaVencimiento", carnet.FechaVencimiento.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@aptoFisico", carnet.AptoFisico ? 1 : 0),
                new SQLiteParameter("@socioId", carnet.SocioId),
                new SQLiteParameter("@id", carnet.Id));
        }

        // Elimina un carnet por su ID
        public void Delete(int id)
        {
            var sql = "DELETE FROM Carnets WHERE Id = @id"; // SQL para eliminar
            _dbHelper.ExecuteNonQuery(sql, new SQLiteParameter("@id", id)); // Ejecuta
        }

        // Obtiene el carnet asociado a un socio específico
        public Carnet GetBySocioId(int socioId)
        {
            // Consulta SQL para buscar por ID de socio
            var sql = "SELECT Id, NroCarnet, FechaEmision, FechaVencimiento, AptoFisico, SocioId FROM Carnets WHERE SocioId = @socioId";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@socioId", socioId));

            if (dt.Rows.Count == 0) return null; // Retorna null si no hay resultados

            var row = dt.Rows[0]; // Toma la primera fila
            return new Carnet // Crea y retorna el objeto Carnet
            {
                Id = (int)(long)row["Id"],
                NroCarnet = (int)(long)row["NroCarnet"],
                FechaEmision = DateTime.Parse(row["FechaEmision"].ToString()),
                FechaVencimiento = DateTime.Parse(row["FechaVencimiento"].ToString()),
                AptoFisico = Convert.ToBoolean((long)row["AptoFisico"]),
                SocioId = (int)(long)row["SocioId"]
            };
        }

        // Obtiene el próximo número de carnet disponible
        public int GetNextCarnetNumber()
        {
            var sql = "SELECT MAX(NroCarnet) FROM Carnets"; // SQL para obtener el máximo número
            var dt = _dbHelper.ExecuteQuery(sql); // Ejecuta la consulta

            if (dt.Rows[0][0] == DBNull.Value) // Si no hay carnets
            {
                return 1000; // Retorna número inicial
            }

            return (int)(long)dt.Rows[0][0] + 1; // Retorna el máximo + 1
        }
    }
}