using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ClubDeportivo
{
    public class DatabaseHelper : IDisposable
    {
        private readonly SQLiteConnection _connection;
        private bool _disposed = false;

       
        public DatabaseHelper(string dbPath = "ClubDB.sqlite")
        {
            string connectionString = string.Format("Data Source={0};Version=3;", dbPath);
            _connection = new SQLiteConnection(connectionString);

         

            try
            {
                _connection.Open();
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Error al conectar con la base de datos: " + ex.Message);
            }
        }



        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (var cmd = new SQLiteCommand(sql, _connection))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            var dt = new DataTable();
            using (var cmd = new SQLiteCommand(sql, _connection))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (var cmd = new SQLiteCommand(sql, _connection))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteScalar();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        if (_connection.State != ConnectionState.Closed)
                            _connection.Close();
                        _connection.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        ~DatabaseHelper()
        {
            Dispose(false);
        }
    }
}

/*
DatabaseHelper (Utilidad)

- Clase auxiliar para operaciones con SQLite
- Maneja conexión, comandos y parámetros
- Proporciona métodos para consultas y ejecuciones
*/