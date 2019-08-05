﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Migrations.Product
{
    [DbContext(typeof(ProductContext))]
    [Migration("20190804135932_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("U.ProductService.Domain.Aggregates.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("MimeType");

                    b.Property<Guid?>("ProductId");

                    b.Property<string>("SeoFilename")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Products_Pictures","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Aggregates.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BarCode")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsPublished");

                    b.Property<DateTime?>("LastFullUpdateDateTime");

                    b.Property<Guid>("ManufacturerId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price");

                    b.HasKey("Id");

                    b.ToTable("products","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Aggregates.Picture", b =>
                {
                    b.HasOne("U.ProductService.Domain.Aggregates.Product")
                        .WithMany("Pictures")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("U.ProductService.Domain.Aggregates.Product", b =>
                {
                    b.OwnsOne("U.ProductService.Domain.Aggregates.Dimensions", "Dimensions", b1 =>
                        {
                            b1.Property<Guid>("ProductId");

                            b1.Property<decimal>("Height");

                            b1.Property<decimal>("Length");

                            b1.Property<decimal>("Weight");

                            b1.Property<decimal>("Width");

                            b1.HasKey("ProductId");

                            b1.ToTable("products","Products");

                            b1.HasOne("U.ProductService.Domain.Aggregates.Product")
                                .WithOne("Dimensions")
                                .HasForeignKey("U.ProductService.Domain.Aggregates.Dimensions", "ProductId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}