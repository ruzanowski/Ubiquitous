using Microsoft.EntityFrameworkCore.Design;
using U.Common.NetCore.EF;

namespace U.IdentityService.Persistance.Contexts.Factories
{
    public class IdentityContextDesignFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<IdentityContext>("../../../..");

            return new IdentityContext(optionsBuilder.Options);
        }
    }
}