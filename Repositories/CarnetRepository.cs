using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    public class CarnetRepository : ICarnetRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public CarnetRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Carnets (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        NroCarnet INTEGER NOT NULL,
                        FechaEmision TEXT NOT NULL,
                        FechaVencimiento TEXT NOT NULL,
                        AptoFisico INTEGER NOT NULL,
                        SocioId INTEGER NOT NULL,
                        FOREIGN KEY(SocioId) REFERENCES Socios(Id))";
            _dbHelper.ExecuteNonQuery(sql);
        }

        

        public Carnet GetById(int id)
        {
            var sql = "SELECT Id, NroCarnet, FechaEmision, FechaVencimiento, AptoFisico, SocioId FROM Carnets WHERE Id = @id";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@id", id));

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new Carnet
            {
                Id = (int)(long)row["Id"],
                NroCarnet = (int)(long)row["NroCarnet"],
                FechaEmision = DateTime.Parse(row["FechaEmision"].ToString()),
                FechaVencimiento = DateTime.Parse(row["FechaVencimiento"].ToString()),
                AptoFisico = Convert.ToBoolean((long)row["AptoFisico"]),
                SocioId = (int)(long)row["SocioId"]
            };
        }

        public IEnumerable<Carnet> GetAll()
        {
            var carnets = new List<Carnet>();
            var sql = "SELECT Id, NroCarnet, FechaEmision, FechaVencimiento, AptoFisico, SocioId FROM Carnets";
            var dt = _dbHelper.ExecuteQuery(sql);

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
            return carnets;
        }

        public void Add(Carnet carnet)
        {
            var sql = @"INSERT INTO Carnets (NroCarnet, FechaEmision, FechaVencimiento, AptoFisico, SocioId) 
                        VALUES (@nroCarnet, @fechaEmision, @fechaVencimiento, @aptoFisico, @socioId)";
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nroCarnet", carnet.NroCarnet),
                new SQLiteParameter("@fechaEmision", carnet.FechaEmision.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@fechaVencimiento", carnet.FechaVencimiento.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@aptoFisico", carnet.AptoFisico ? 1 : 0),
                new SQLiteParameter("@socioId", carnet.SocioId));
        }

        public void Update(Carnet carnet)
        {
            var sql = @"UPDATE Carnets SET 
                        NroCarnet = @nroCarnet,
                        FechaEmision = @fechaEmision,
                        FechaVencimiento = @fechaVencimiento,
                        AptoFisico = @aptoFisico,
                        SocioId = @socioId
                        WHERE Id = @id";
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@nroCarnet", carnet.NroCarnet),
                new SQLiteParameter("@fechaEmision", carnet.FechaEmision.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@fechaVencimiento", carnet.FechaVencimiento.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@aptoFisico", carnet.AptoFisico ? 1 : 0),
                new SQLiteParameter("@socioId", carnet.SocioId),
                new SQLiteParameter("@id", carnet.Id));
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM Carnets WHERE Id = @id";
            _dbHelper.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
        }

        public Carnet GetBySocioId(int socioId)
        {
            var sql = "SELECT Id, NroCarnet, FechaEmision, FechaVencimiento, AptoFisico, SocioId FROM Carnets WHERE SocioId = @socioId";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@socioId", socioId));

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new Carnet
            {
                Id = (int)(long)row["Id"],
                NroCarnet = (int)(long)row["NroCarnet"],
                FechaEmision = DateTime.Parse(row["FechaEmision"].ToString()),
                FechaVencimiento = DateTime.Parse(row["FechaVencimiento"].ToString()),
                AptoFisico = Convert.ToBoolean((long)row["AptoFisico"]),
                SocioId = (int)(long)row["SocioId"]
            };
        }

        public int GetNextCarnetNumber()
        {
            var sql = "SELECT MAX(NroCarnet) FROM Carnets";
            var dt = _dbHelper.ExecuteQuery(sql);

            if (dt.Rows[0][0] == DBNull.Value)
            {
                return 1000; // Número inicial si no hay carnets
            }

            return (int)(long)dt.Rows[0][0] + 1;
        }
        
    }
}