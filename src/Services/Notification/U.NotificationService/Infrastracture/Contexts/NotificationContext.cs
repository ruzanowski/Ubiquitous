using Microsoft.EntityFrameworkCore;
using U.NotificationService.Domain;
using U.NotificationService.Infrastracture.EntityConfigurations;

namespace U.NotificationService.Infrastracture.Contexts
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
