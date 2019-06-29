﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using U.FetchService.Persistance.Context;

namespace U.FetchService.Persistance.Migrations
{
    [DbContext(typeof(UmContext))]
    [Migration("20190611205509_init2")]
    partial class init2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("U.FetchService.Domain.Entities.Common.Modified", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Modified");
                });

            modelBuilder.Entity("U.FetchService.Domain.Entities.Picture.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<string>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Picture");
                });

            modelBuilder.Entity("U.FetchService.Domain.Entities.Product.Product", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<string>("CountryMade");

                    b.Property<int?>("CreatedId");

                    b.Property<string>("Description");

                    b.Property<decimal>("Height");

                    b.Property<int>("InStock");

                    b.Property<bool>("IsPublished");

                    b.Property<int?>("LastModifiedId");

                    b.Property<decimal>("Length");

                    b.Property<int?>("MainPictureId");

                    b.Property<int>("ManufacturerId");

                    b.Property<string>("ManufacturerPartNumber");

                    b.Property<string>("Name");

                    b.Property<decimal>("PriceInTax");

                    b.Property<decimal>("PriceMinimumSpecifiedByCustomer");

                    b.Property<decimal>("ProductCost");

                    b.Property<string>("ProductUniqueCode");

                    b.Property<int>("TaxCategoryId");

                    b.Property<string>("UrlSlug");

                    b.Property<decimal>("Weight");

                    b.Property<decimal>("Width");

                    b.HasKey("Id");

                    b.HasIndex("CreatedId");

                    b.HasIndex("LastModifiedId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("U.FetchService.Domain.Entities.ProductTags.ProductTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProductId");

                    b.Property<string>("Tag");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductTag");
                });

            modelBuilder.Entity("U.FetchService.Domain.Entities.Picture.Picture", b =>
                {
                    b.HasOne("U.FetchService.Domain.Entities.Product.Product")
                        .WithMany("Pictures")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("U.FetchService.Domain.Entities.Product.Product", b =>
                {
                    b.HasOne("U.FetchService.Domain.Entities.Common.Modified", "Created")
                        .WithMany()
                        .HasForeignKey("CreatedId");

                    b.HasOne("U.FetchService.Domain.Entities.Common.Modified", "LastModified")
                        .WithMany()
                        .HasForeignKey("LastModifiedId");
                });

            modelBuilder.Entity("U.FetchService.Domain.Entities.ProductTags.ProductTag", b =>
                {
                    b.HasOne("U.FetchService.Domain.Entities.Product.Product")
                        .WithMany("ProductTags")
                        .HasForeignKey("ProductId");
                });
#pragma warning restore 612, 618
        }
    }
}
