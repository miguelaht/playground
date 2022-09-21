using System.Data;
using Microsoft.Data.Sqlite;

namespace IdentityMicroService.Database;

public interface IDBConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}

public class DBConnectionFactory : IDBConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DBConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var sqlConnection = new SqliteConnection();
        sqlConnection.ConnectionString = _configuration.GetConnectionString("main");
        await sqlConnection.OpenAsync();

        return sqlConnection;
    }
}
