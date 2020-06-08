﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Migrations.Product
{
    [DbContext(typeof(ProductContext))]
    partial class ProductContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("U.ProductService.Domain.Common.MimeType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Pictures_MimeTypes","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Manufacturer.Manufacturer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UniqueClientId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Manufacturer.ManufacturerPicture", b =>
                {
                    b.Property<Guid>("ManufacturerPictureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ManufacturerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PictureId")
                        .HasColumnType("uuid");

                    b.HasKey("ManufacturerPictureId");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("PictureId");

                    b.ToTable("Manufacturer_Pictures","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Picture.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FileStorageUploadId")
                        .HasColumnType("uuid");

                    b.Property<int>("MimeTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PictureAddedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MimeTypeId");

                    b.ToTable("Pictures","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Product.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentCategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Products_Categories","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Product.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BarCode")
                        .HasColumnType("text");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ExternalId")
                        .HasColumnType("text");

                    b.Property<string>("ExternalSourceName")
                        .HasColumnType("text");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("text");

                    b.Property<Guid>("ManufacturerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("ProductTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductTypeId");

                    b.HasIndex("ExternalId", "ExternalSourceName")
                        .IsUnique();

                    b.ToTable("Products","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Product.ProductPicture", b =>
                {
                    b.Property<Guid>("ProductPictureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PictureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.HasKey("ProductPictureId");

                    b.HasIndex("PictureId");

                    b.HasIndex("ProductId");

                    b.ToTable("Product_Pictures","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Product.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Products_Types","Products");
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Manufacturer.ManufacturerPicture", b =>
                {
                    b.HasOne("U.ProductService.Domain.Entities.Manufacturer.Manufacturer", "Manufacturer")
                        .WithMany("Pictures")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("U.ProductService.Domain.Entities.Picture.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Picture.Picture", b =>
                {
                    b.HasOne("U.ProductService.Domain.Common.MimeType", "MimeType")
                        .WithMany()
                        .HasForeignKey("MimeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Product.Product", b =>
                {
                    b.HasOne("U.ProductService.Domain.Entities.Product.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("U.ProductService.Domain.Entities.Product.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("U.ProductService.Domain.Dimensions", "Dimensions", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Height")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Length")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Weight")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Width")
                                .HasColumnType("numeric");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });
                });

            modelBuilder.Entity("U.ProductService.Domain.Entities.Product.ProductPicture", b =>
                {
                    b.HasOne("U.ProductService.Domain.Entities.Picture.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("U.ProductService.Domain.Entities.Product.Product", "Product")
                        .WithMany("Pictures")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
