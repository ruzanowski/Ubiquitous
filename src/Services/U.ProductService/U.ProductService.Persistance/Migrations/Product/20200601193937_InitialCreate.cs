using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace U.ProductService.Persistance.Migrations.Product
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Products");

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
                name: "Products_Categories",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ParentCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_Categories", x => x.Id);
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
                name: "Pictures",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileStorageUploadId = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    MimeTypeId = table.Column<int>(nullable: false),
                    PictureAddedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Pictures_MimeTypes_MimeTypeId",
                        column: x => x.MimeTypeId,
                        principalSchema: "Products",
                        principalTable: "Pictures_MimeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Products_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Products",
                        principalTable: "Products_Categories",
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
                name: "Manufacturer_Pictures",
                schema: "Products",
                columns: table => new
                {
                    ManufacturerPictureId = table.Column<Guid>(nullable: false),
                    PictureId = table.Column<Guid>(nullable: false),
                    ManufacturerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer_Pictures", x => x.ManufacturerPictureId);
                    table.ForeignKey(
                        name: "FK_Manufacturer_Pictures_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "Products",
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manufacturer_Pictures_Pictures_PictureId",
                        column: x => x.PictureId,
                        principalSchema: "Products",
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Pictures",
                schema: "Products",
                columns: table => new
                {
                    ProductPictureId = table.Column<Guid>(nullable: false),
                    PictureId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Pictures", x => x.ProductPictureId);
                    table.ForeignKey(
                        name: "FK_Product_Pictures_Pictures_PictureId",
                        column: x => x.PictureId,
                        principalSchema: "Products",
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Pictures_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Products",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturer_Pictures_ManufacturerId",
                schema: "Products",
                table: "Manufacturer_Pictures",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturer_Pictures_PictureId",
                schema: "Products",
                table: "Manufacturer_Pictures",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_MimeTypeId",
                schema: "Products",
                table: "Pictures",
                column: "MimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Pictures_PictureId",
                schema: "Products",
                table: "Product_Pictures",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Pictures_ProductId",
                schema: "Products",
                table: "Product_Pictures",
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
                name: "Manufacturer_Pictures",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Product_Pictures",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Manufacturers",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Pictures",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Pictures_MimeTypes",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Products_Categories",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Products_Types",
                schema: "Products");
        }
    }
}
