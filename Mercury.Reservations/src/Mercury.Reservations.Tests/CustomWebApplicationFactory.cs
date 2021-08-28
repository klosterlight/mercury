using Mercury.Common;
using Mercury.Reservations.Service.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.Reservations.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
            {
                configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                configurationBuilder.AddEnvironmentVariables();
            });

        // if you want to override Physical database with in-memory database
        builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    // .AddMongo()
                    // .AddMongoRepository<Room>("rooms")
                    .BuildServiceProvider();
            });
        }
    }
}