using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartStore.Persistance.Context;

namespace U.SmartStoreAdapter.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        public IConfiguration Configuration { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
                {
                    // Create a new service provider.
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkSqlServer()
                        .BuildServiceProvider();

                    const string dbConnectionString = "Data Source=.;Initial Catalog=SmartStore_Tests;Integrated Security=True";

                    // Add a database context (AppDbContext) using an in-memory database for testing.
                    services.AddDbContext<SmartStoreContext>(options =>
                    {
                        options.UseSqlServer(dbConnectionString);
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database contexts
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var appDb = scopedServices.GetRequiredService<SmartStoreContext>();

                        var logger = scopedServices
                            .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                        // Ensure the database is created.
                        appDb.Database.EnsureCreated();

                        try
                        {
                            // Seed the database with some specific test data.
                            //SeedData.PopulateTestData(appDb);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding the " +
                                                "database with test messages. Error: {ex.Message}");
                        }
                    }
                });
        }
    }
}