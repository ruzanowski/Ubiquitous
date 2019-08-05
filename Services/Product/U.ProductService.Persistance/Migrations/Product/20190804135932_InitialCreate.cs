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
                name: "products",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    BarCode = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    LastFullUpdateDateTime = table.Column<DateTime>(nullable: true),
                    Dimensions_Length = table.Column<decimal>(nullable: false),
                    Dimensions_Width = table.Column<decimal>(nullable: false),
                    Dimensions_Height = table.Column<decimal>(nullable: false),
                    Dimensions_Weight = table.Column<decimal>(nullable: false),
                    ManufacturerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products_Pictures",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SeoFilename = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    MimeType = table.Column<string>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Pictures_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Products",
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Pictures_ProductId",
                schema: "Products",
                table: "Products_Pictures",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products_Pictures",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "products",
                schema: "Products");
        }
    }
}
