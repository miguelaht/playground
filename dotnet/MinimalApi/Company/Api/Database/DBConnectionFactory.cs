using System.Data;
using System.Data.Common;
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

        public DBConnectionFactory(IConfiguration confiuration)
        {
            _confiuration = confiuration;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = _confiuration.GetConnectionString("main");
            await sqlConnection.OpenAsync();

            return sqlConnection;
        }
    }
}

