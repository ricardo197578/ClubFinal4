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

        // Constructor que recibe el helper y asegura la creación de la tabla Pagos
        public PagoRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase(); // Crear tabla si no existe
        }

        // Método privado para crear la tabla Pagos si no existe aún
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

        // Método para registrar un nuevo pago en la base de datos
        public void RegistrarPago(Pago pago)
        {
            var sql = @"INSERT INTO Pagos 
                       (NoSocioId, ActividadId, Monto, FechaPago, Metodo) 
                       VALUES (@noSocioId, @actividadId, @monto, @fecha, @metodo)";

            // Ejecuta la inserción con parámetros para prevenir inyección SQL
            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@noSocioId", pago.NoSocioId), // Id del no socio que paga
                new SQLiteParameter("@actividadId", pago.ActividadId), // Id de la actividad pagada
                new SQLiteParameter("@monto", pago.Monto), // Monto pagado
                new SQLiteParameter("@fecha", pago.FechaPago.ToString("yyyy-MM-dd HH:mm:ss")), // Fecha y hora del pago en formato string
                new SQLiteParameter("@metodo", (int)pago.Metodo)); // Método de pago convertido a entero
        }
    }
}
