using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using U.IntegrationEventLog;

namespace U.ProductService.Persistance.Contexts.Factories
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class IntegrationEventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationEventLogContext>
    {
        public IntegrationEventLogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IntegrationEventLogContext>();
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory + @"../../../../U.ProductService")
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = configuration.GetSection("IntegrationEventLogConnection").Value;

            optionsBuilder.UseNpgsql(connection,
                options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

            return new IntegrationEventLogContext(optionsBuilder.Options);
        }
    }
}