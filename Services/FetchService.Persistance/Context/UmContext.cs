using Microsoft.EntityFrameworkCore;
using U.FetchService.Domain.Entities.Product;

namespace U.FetchService.Persistance.Context
{
    public class UmContext : DbContext
    {
        public UmContext(DbContextOptions options): base(options)
        {
        }
        
       public DbSet<Product> Products { get; set; }
    }
}