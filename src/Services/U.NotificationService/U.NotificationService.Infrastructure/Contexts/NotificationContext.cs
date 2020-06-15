using Microsoft.EntityFrameworkCore;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.EntityConfigurations;
using Z.EntityFramework.Extensions;

namespace U.NotificationService.Infrastructure.Contexts
{
    public class NotificationContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "Notifications";

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }

        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
            EntityFrameworkManager.ContextFactory = context => this;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfirmationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
        }

    }

    public class Translation
    {
        public string CompositeKey { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
    }
}
