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
        private readonly IConfiguration _configuration;
        private readonly string _connectionStringKey;

        public DBConnectionFactory(IConfiguration configuration, string connectionStringKey)
        {
            _configuration = configuration;
            _connectionStringKey = connectionStringKey;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = _configuration.GetConnectionString(
                _connectionStringKey
            );
            await sqlConnection.OpenAsync();

            return sqlConnection;
        }
    }
}
