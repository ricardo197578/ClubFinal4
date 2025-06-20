using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public PagoRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Pagos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        NoSocioId INTEGER NOT NULL,
                        ActividadId INTEGER NOT NULL,
                        Monto REAL NOT NULL,
                        FechaPago TEXT NOT NULL,
                        Metodo INTEGER NOT NULL,
                        FOREIGN KEY(NoSocioId) REFERENCES NoSocios(Id),
                        FOREIGN KEY(ActividadId) REFERENCES Actividades(Id))";
            _dbHelper.ExecuteNonQuery(sql);
        }

        public void RegistrarPago(Pago pago)
        {
            var sql = @"INSERT INTO Pagos 
                       (NoSocioId, ActividadId, Monto, FechaPago, Metodo) 
                       VALUES (@noSocioId, @actividadId, @monto, @fecha, @metodo)";
            
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@noSocioId", pago.NoSocioId),
                new SQLiteParameter("@actividadId", pago.ActividadId),
                new SQLiteParameter("@monto", pago.Monto),
                new SQLiteParameter("@fecha", pago.FechaPago.ToString("yyyy-MM-dd HH:mm:ss")),
                new SQLiteParameter("@metodo", (int)pago.Metodo));
        }
    }
}