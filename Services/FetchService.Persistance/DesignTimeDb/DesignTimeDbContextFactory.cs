using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design;
using U.Common.Database;
using U.FetchService.Persistance.Context;

namespace U.FetchService.Persistance.DesignTimeDb
{
    [SuppressMessage("ReSharper", "RedundantCaseLabel")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")] 
    //it's only used during designing migrations
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FetchServiceContext>
    {
        public FetchServiceContext CreateDbContext(string[] args)
        {
            return new FetchServiceContext(ContextDesigner
                .CreateDbContextOptionsBuilder<FetchServiceContext>(@"../../../../FetchService").Options);
        }
    }
}