using Microsoft.EntityFrameworkCore.Design;
using SmartStore.Persistance.Context;
using U.Common;

namespace SmartStore.Persistance.DesignTimeDb
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SmartStoreContext>
    {
        public SmartStoreContext CreateDbContext(string[] args)
        {
            return ContextDesigner.CreateDbContext<SmartStoreContext>("../../../../U.SmartStoreAdapter");
        }
    }
}