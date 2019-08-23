using Microsoft.EntityFrameworkCore.Migrations;

namespace U.ProductService.Persistance.Migrations.Product
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastFullUpdateDateTime",
                schema: "Products",
                table: "products",
                newName: "LastUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                schema: "Products",
                table: "products",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Products",
                table: "products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "Products",
                table: "products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Products",
                table: "products");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "Products",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                schema: "Products",
                table: "products",
                newName: "LastFullUpdateDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "Products",
                table: "products",
                newName: "CreatedDateTime");
        }
    }
}
