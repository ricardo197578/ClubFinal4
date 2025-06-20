using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    // Repositorio para manejar la persistencia de pagos en la base de datos
    public class PagoRepository : IPagoRepository
    {
        // Instancia de helper para acceso a base de datos SQLite
        private readonly DatabaseHelper _dbHelper;

        // Constructor que recibe el helper y asegura la creaci�n de la tabla Pagos
        public PagoRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase(); // Crear tabla si no existe
        }

        // M�todo privado para crear la tabla Pagos si no existe a�n
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

        // M�todo para registrar un nuevo pago en la base de datos
        public void RegistrarPago(Pago pago)
        {
            var sql = @"INSERT INTO Pagos 
                       (NoSocioId, ActividadId, Monto, FechaPago, Metodo) 
                       VALUES (@noSocioId, @actividadId, @monto, @fecha, @metodo)";

            // Ejecuta la inserci�n con par�metros para prevenir inyecci�n SQL
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@noSocioId", pago.NoSocioId), // Id del no socio que paga
                new SQLiteParameter("@actividadId", pago.ActividadId), // Id de la actividad pagada
                new SQLiteParameter("@monto", pago.Monto), // Monto pagado
                new SQLiteParameter("@fecha", pago.FechaPago.ToString("yyyy-MM-dd HH:mm:ss")), // Fecha y hora del pago en formato string
                new SQLiteParameter("@metodo", (int)pago.Metodo)); // M�todo de pago convertido a entero
        }
    }
}
