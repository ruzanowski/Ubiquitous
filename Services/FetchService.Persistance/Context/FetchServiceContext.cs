using Microsoft.EntityFrameworkCore;
using U.FetchService.Domain.Entities;

namespace U.FetchService.Persistance.Context
{
    public class FetchServiceContext : DbContext
    {
        public FetchServiceContext(DbContextOptions<FetchServiceContext> options): base(options)
        {
        }
        
        public DbSet<BatchTransaction> Transactions { get; set; }
    }
}