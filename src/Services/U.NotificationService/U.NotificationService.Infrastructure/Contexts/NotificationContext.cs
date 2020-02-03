using Microsoft.EntityFrameworkCore;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.EntityConfigurations;

namespace U.NotificationService.Infrastructure.Contexts
{
    public class NotificationContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "Notifications";

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }

        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfirmationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
        }

    }
}
