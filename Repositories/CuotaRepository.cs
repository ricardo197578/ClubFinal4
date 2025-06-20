using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClubDeportivo.Interfaces;
using ClubDeportivo.Models;

namespace ClubDeportivo.Repositories
{
    // Repositorio para la gestión de cuotas de socios
    public class CuotaRepository : ICuotaRepository
    {
        // Helper para operaciones con la base de datos
        private readonly DatabaseHelper _dbHelper;
        // Valor por defecto para las cuotas
        private const decimal ValorCuotaDefault = 5000.00m;

        // Constructor que recibe el DatabaseHelper como dependencia
        public CuotaRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            InitializeDatabase(); // Inicializa la estructura de la base de datos
        }

        // Método para crear la tabla de cuotas si no existe
        private void InitializeDatabase()
        {
            // SQL para crear la tabla Cuotas con sus campos y relaciones
            var sql = @"CREATE TABLE IF NOT EXISTS Cuotas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SocioId INTEGER NOT NULL,
                        Monto REAL NOT NULL,
                        FechaPago TEXT NOT NULL,
                        FechaVencimiento TEXT NOT NULL,
                        Pagada INTEGER NOT NULL,
                        MetodoPago INTEGER NOT NULL,
                        Periodo TEXT NOT NULL,
                        FOREIGN KEY(SocioId) REFERENCES Socios(Id))";
            _dbHelper.ExecuteNonQuery(sql); // Ejecuta el comando SQL
        }

        // Método para registrar el pago de una cuota
        public void RegistrarPagoCuota(int socioId, decimal monto, DateTime fechaPago,
                             MetodoPago metodo)
        {
            // Obtener la fecha actual del socio
            DateTime fechaVencimientoActual = ObtenerFechaVencimientoActual(socioId);

            // 1. Registrar el pago en la tabla Cuotas
            var sqlInsert = @"INSERT INTO Cuotas 
                            (SocioId, Monto, FechaPago, FechaVencimiento, Pagada, MetodoPago, Periodo) 
                            VALUES 
                            (@socioId, @monto, @fechaPago, @fechaVencimiento, 1, @metodo, @periodo)";

            _dbHelper.ExecuteNonQuery(sqlInsert,
                new SQLiteParameter("@socioId", socioId),
                new SQLiteParameter("@monto", monto),
                new SQLiteParameter("@fechaPago", fechaPago.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@fechaVencimiento", fechaVencimientoActual.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@metodo", (int)metodo),
                new SQLiteParameter("@periodo", fechaVencimientoActual.ToString("yyyy-MM")));

            // 2. Calcular nuevo vencimiento (30 días después del actual)
            ActualizarFechaVencimiento(socioId, fechaVencimientoActual.AddDays(30));

            // 3. Actualizar la fecha en la tabla Socios
            //ActualizarFechaVencimiento(socioId, nuevoVencimiento);

            // 4. Activar al socio si estaba inactivo
            ActivarSocio(socioId);
        }

        // Método para buscar un socio activo por su DNI
        public Socio BuscarSocioActivoPorDni(string dni)
        {
            var sql = @"SELECT Id, Nombre, Apellido, Dni, FechaInscripcion, 
                        FechaVencimientoCuota, EstadoActivo, Tipo
                        FROM Socios 
                        WHERE Dni = @dni AND EstadoActivo = 1";

            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@dni", dni));

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new Socio
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Dni = row["Dni"].ToString(),
                FechaInscripcion = DateTime.Parse(row["FechaInscripcion"].ToString()),
                FechaVencimientoCuota = DateTime.Parse(row["FechaVencimientoCuota"].ToString()),
                EstadoActivo = Convert.ToBoolean(row["EstadoActivo"]),
                Tipo = (TipoSocio)Convert.ToInt32(row["Tipo"])
            };
        }

        // Método para obtener la fecha de vencimiento actual de un socio
        public DateTime ObtenerFechaVencimientoActual(int socioId)
        {
            var sql = "SELECT FechaVencimientoCuota FROM Socios WHERE Id = @socioId";
            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@socioId", socioId));

            if (dt.Rows.Count == 0)
                throw new KeyNotFoundException("Socio no encontrado");

            return DateTime.Parse(dt.Rows[0]["FechaVencimientoCuota"].ToString());
        }

        // Método para actualizar la fecha de vencimiento de un socio
        public void ActualizarFechaVencimiento(int socioId, DateTime nuevaFecha)
        {
            var sql = @"UPDATE Socios SET 
                        FechaVencimientoCuota = @fechaVencimiento
                        WHERE Id = @socioId";

            _dbHelper.ExecuteNonQuery(sql,
                new SQLiteParameter("@fechaVencimiento", nuevaFecha.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@socioId", socioId));
        }

        // Método para activar un socio
        public void ActivarSocio(int socioId)
        {
            var sql = "UPDATE Socios SET EstadoActivo = 1 WHERE Id = @socioId";
            _dbHelper.ExecuteNonQuery(sql, new SQLiteParameter("@socioId", socioId));
        }

        // Método para obtener las cuotas por vencer hasta una fecha límite
        public IEnumerable<Cuota> ObtenerCuotasPorVencer(DateTime fechaLimite)
        {
            var cuotas = new List<Cuota>();
            var sql = @"SELECT Id, SocioId, Monto, FechaPago, FechaVencimiento, Pagada, Metodo, Periodo
                        FROM Cuotas
                        WHERE FechaVencimiento <= @fechaLimite AND Pagada = 0";

            var dt = _dbHelper.ExecuteQuery(sql,
                new SQLiteParameter("@fechaLimite", fechaLimite.ToString("yyyy-MM-dd")));

            foreach (System.Data.DataRow row in dt.Rows)
            {
                cuotas.Add(new Cuota
                {
                    Id = Convert.ToInt32(row["Id"]),
                    SocioId = Convert.ToInt32(row["SocioId"]),
                    Monto = Convert.ToDecimal(row["Monto"]),
                    FechaPago = DateTime.Parse(row["FechaPago"].ToString()),
                    FechaVencimiento = DateTime.Parse(row["FechaVencimiento"].ToString()),
                    Pagada = Convert.ToBoolean(row["Pagada"]),
                    Metodo = (MetodoPago)Convert.ToInt32(row["Metodo"]),
                    Periodo = row["Periodo"].ToString()
                });
            }
            return cuotas;
        }

        // Método para obtener todas las cuotas de un socio específico
        public IEnumerable<Cuota> ObtenerCuotasPorSocio(int socioId)
        {
            var cuotas = new List<Cuota>();
            var sql = @"SELECT Id, SocioId, Monto, FechaPago, FechaVencimiento, Pagada, Metodo, Periodo
                        FROM Cuotas
                        WHERE SocioId = @socioId
                        ORDER BY FechaVencimiento DESC";

            var dt = _dbHelper.ExecuteQuery(sql, new SQLiteParameter("@socioId", socioId));

            foreach (System.Data.DataRow row in dt.Rows)
            {
                cuotas.Add(new Cuota
                {
                    Id = Convert.ToInt32(row["Id"]),
                    SocioId = Convert.ToInt32(row["SocioId"]),
                    Monto = Convert.ToDecimal(row["Monto"]),
                    FechaPago = DateTime.Parse(row["FechaPago"].ToString()),
                    FechaVencimiento = DateTime.Parse(row["FechaVencimiento"].ToString()),
                    Pagada = Convert.ToBoolean(row["Pagada"]),
                    Metodo = (MetodoPago)Convert.ToInt32(row["Metodo"]),
                    Periodo = row["Periodo"].ToString()
                });
            }
            return cuotas;
        }

        // Método para obtener el valor actual de la cuota
        public decimal ObtenerValorActualCuota()
        {
            return ValorCuotaDefault; // Retorna el valor por defecto
        }
    }
}