using System.Data;
using System.Data.SqlClient;

namespace Api.Database
{
    public interface IDBConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }

    public class DBConnectionFactory : IDBConnectionFactory
    {
        private readonly IConfiguration _confiuration;
        private readonly string _connectionStringKey;
        public DBConnectionFactory(IConfiguration confiuration, string connectionStringKey)
        {
            _confiuration = confiuration;
            _connectionStringKey = connectionStringKey;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = _confiuration.GetConnectionString(_connectionStringKey);
            await sqlConnection.OpenAsync();

            return sqlConnection;
        }
    }
}

