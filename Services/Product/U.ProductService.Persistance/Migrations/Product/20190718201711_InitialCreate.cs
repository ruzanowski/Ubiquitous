using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace U.ProductService.Persistance.Migrations.Product
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "product");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductDueDate = table.Column<DateTime>(nullable: false),
                    Address_Street = table.Column<string>(nullable: true),
                    Address_City = table.Column<string>(nullable: true),
                    Address_State = table.Column<string>(nullable: true),
                    Address_Country = table.Column<string>(nullable: true),
                    Address_ZipCode = table.Column<string>(nullable: true),
                    BuyerId = table.Column<int>(nullable: true),
                    ManufacturerId = table.Column<Guid>(nullable: false),
                    IsDraft = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "product");
        }
    }
}
