using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using U.Common.Database;
using U.Common.Mvc;
using U.IntegrationEventLog;

namespace U.ProductService.Persistance.Contexts.Factories
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class IntegrationEventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationEventLogContext>
    {
        public IntegrationEventLogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<IntegrationEventLogContext>("../../../../U.ProductService");
            
            return new IntegrationEventLogContext(optionsBuilder.Options);
        }
    }
}