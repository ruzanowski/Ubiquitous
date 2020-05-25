using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design;
using U.Common.NetCore.Database;

namespace U.IntegrationEventLog
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class IntegrationEventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationEventLogContext>
    {
        public IntegrationEventLogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<IntegrationEventLogContext>("../../../");

            return new IntegrationEventLogContext(optionsBuilder.Options);
        }
    }
}