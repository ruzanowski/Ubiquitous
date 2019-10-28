﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.NotificationService.Domain;
using U.NotificationService.Infrastracture.Contexts;

namespace U.NotificationService.Infrastracture.EntityConfigurations
{
    class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification", NotificationContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            
            builder.Property(x => x.CreationDate).IsRequired();

            builder.HasMany(x => x.Confirmations)
                .WithOne(z => z.Notification)
                .IsRequired(false);

            builder.HasOne(o => o.IntegrationEvent)
                .WithMany()
                .IsRequired();
        }
    }
}
