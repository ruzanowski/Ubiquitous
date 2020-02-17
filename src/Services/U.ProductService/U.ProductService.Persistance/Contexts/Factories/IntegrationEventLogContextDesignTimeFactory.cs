using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design;
using U.Common.Database;
using U.IntegrationEventLog;

namespace U.ProductService.Persistance.Contexts.Factories
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class IntegrationEventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationEventLogContext>
    {
        public IntegrationEventLogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<IntegrationEventLogContext>("../../../U.ProductService");

            return new IntegrationEventLogContext(optionsBuilder.Options);
        }
    }
}