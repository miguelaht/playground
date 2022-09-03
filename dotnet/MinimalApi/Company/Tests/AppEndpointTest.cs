using Api.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Tests;

public class AppEndpointTest : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        builder.UseKestrel();

        builder.ConfigureServices(services =>
        {
            var dbService = services.SingleOrDefault(
                d => d.ServiceType == typeof(IDBConnectionFactory)
            );
            services.Remove(dbService!);

            services.TryAddSingleton<IDBConnectionFactory>(
                provider => new DBConnectionFactory(config, "testing")
            );
        });
    }
}
