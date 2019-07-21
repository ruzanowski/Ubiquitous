using Microsoft.EntityFrameworkCore;
using U.FetchService.Domain;

namespace U.FetchService.Infrastructure.Context
{
    public class FetchServiceContext : DbContext
    {
        public FetchServiceContext(DbContextOptions<FetchServiceContext> options): base(options)
        {
        }
        
        public DbSet<BatchTransaction> Transactions { get; set; }
    }
}