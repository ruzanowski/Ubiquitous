using Microsoft.EntityFrameworkCore;
using U.IdentityService.Domain.Domain;

namespace U.IdentityService.Persistance.Contexts
{
    public class IdentityContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "Identity";

        //db sets
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
    }
}