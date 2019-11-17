﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using U.NotificationService.Infrastracture.Contexts;

namespace U.NotificationService.Infrastracture.Migrations
{
    [DbContext(typeof(NotificationContext))]
    [Migration("20191102102122_InitialCreate2")]
    partial class InitialCreate2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("U.NotificationService.Domain.Confirmation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ConfirmationDate");

                    b.Property<int>("ConfirmationType");

                    b.Property<Guid>("DeviceReceivedId");

                    b.Property<Guid>("NotificationId");

                    b.Property<Guid>("User");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId");

                    b.HasIndex("User");

                    b.ToTable("Notification_confirmation","Notifications");
                });

            modelBuilder.Entity("U.NotificationService.Domain.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("IntegrationEvent")
                        .IsRequired();

                    b.Property<Guid>("IntegrationEventId");

                    b.Property<int>("IntegrationEventType");

                    b.HasKey("Id");

                    b.ToTable("Notification","Notifications");
                });

            modelBuilder.Entity("U.NotificationService.Domain.Confirmation", b =>
                {
                    b.HasOne("U.NotificationService.Domain.Notification")
                        .WithMany("Confirmations")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
