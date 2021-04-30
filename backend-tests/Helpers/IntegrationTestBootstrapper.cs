using System;
using System.Linq;
using System.Net.Http;
using DataStores;
using ef_issue;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Helpers
{

    public class IntegrationTestBootstrapper
    {
        public static (HttpClient, ApplicationDbContext) Build()
        {
            var factory = new WebApplicationFactory<Startup>();
            var databaseName = Guid.NewGuid().ToString();

            // Arrange
            var configuredFactory =
             factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var old = services.Single(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    services.Remove(old);
                    // Force in memory database
                    services.AddDbContext<ApplicationDbContext>(options =>
                   {
                       options.UseInMemoryDatabase(databaseName: databaseName);
                   });
                });
                builder.UseDefaultServiceProvider(options => options.ValidateOnBuild = true);
                builder.ConfigureLogging((hostingContext, logging) =>
                {
                    // Remove unwanted loggers
                    logging.ClearProviders();
                });
            });

            var httpClient = configuredFactory.CreateClient();
            var dbContext = configuredFactory.Services.GetRequiredService<ApplicationDbContext>();

            return (httpClient, dbContext);
        }
    }
}