using Microsoft.EntityFrameworkCore.Design;
using U.Common.Database;

namespace U.IdentityService.Persistance.Contexts.Factories
{
    public class IdentityContextDesignFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<IdentityContext>("../../../U.IdentityService");

            return new IdentityContext(optionsBuilder.Options);
        }
    }
}