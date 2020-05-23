using Microsoft.EntityFrameworkCore.Design;
using SmartStore.Persistance.Context;
using U.Common.NetCore.Database;

namespace SmartStore.Persistance.DesignTimeDb
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SmartStoreContext>
    {
        public SmartStoreContext CreateDbContext(string[] args)
        {
            return new SmartStoreContext(ContextDesigner
                .CreateDbContextOptionsBuilder<SmartStoreContext>("../../../../../U.SmartStoreAdapter").Options);
        }
    }
}