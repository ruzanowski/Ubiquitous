using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design;
using U.Common;
using U.FetchService.Persistance.Context;

namespace U.FetchService.Persistance.DesignTimeDb
{
    [SuppressMessage("ReSharper", "RedundantCaseLabel")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")] 
    //it's only used during designing migrations
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UmContext>
    {
        public UmContext CreateDbContext(string[] args)
        {
            return ContextDesigner.CreateDbContext<UmContext>(@"../../../../U.FetchService");
        }
    }
}