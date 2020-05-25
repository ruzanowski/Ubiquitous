using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace U.ProductService.Persistance.Migrations.Product
{
    public partial class InitialCreate31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Products");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ParentCategoryId = table.Column<Guid>(nullable: true),
                    IsDraft = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    UniqueClientId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures_MimeTypes",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures_MimeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products_Types",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    BarCode = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    Dimensions_Length = table.Column<decimal>(nullable: true),
                    Dimensions_Width = table.Column<decimal>(nullable: true),
                    Dimensions_Height = table.Column<decimal>(nullable: true),
                    Dimensions_Weight = table.Column<decimal>(nullable: true),
                    ManufacturerId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    ProductTypeId = table.Column<int>(nullable: false),
                    ExternalSourceName = table.Column<string>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Products",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Products_Types_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalSchema: "Products",
                        principalTable: "Products_Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileStorageUploadId = table.Column<Guid>(nullable: false),
                    AggregateRootId = table.Column<Guid>(nullable: false),
                    AggregateRootName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    MimeTypeId = table.Column<int>(nullable: true),
                    ManufacturerId = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "Products",
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pictures_Pictures_MimeTypes_MimeTypeId",
                        column: x => x.MimeTypeId,
                        principalSchema: "Products",
                        principalTable: "Pictures_MimeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pictures_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Products",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ManufacturerId",
                schema: "Products",
                table: "Pictures",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_MimeTypeId",
                schema: "Products",
                table: "Pictures",
                column: "MimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ProductId",
                schema: "Products",
                table: "Pictures",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "Products",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                schema: "Products",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ExternalId_ExternalSourceName",
                schema: "Products",
                table: "Products",
                columns: new[] { "ExternalId", "ExternalSourceName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Manufacturers",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Pictures_MimeTypes",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Products_Types",
                schema: "Products");
        }
    }
}
