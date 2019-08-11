using Microsoft.EntityFrameworkCore.Migrations;

namespace U.ProductService.Persistance.Migrations.Product
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_products_BarCode",
                schema: "Products",
                table: "products",
                column: "BarCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_products_BarCode",
                schema: "Products",
                table: "products");
        }
    }
}
