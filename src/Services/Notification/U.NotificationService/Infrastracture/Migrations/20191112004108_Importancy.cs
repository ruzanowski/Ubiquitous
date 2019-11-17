using Microsoft.EntityFrameworkCore.Migrations;

namespace U.NotificationService.Infrastracture.Migrations
{
    public partial class Importancy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Importancy",
                schema: "Notifications",
                table: "Notification",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Importancy",
                schema: "Notifications",
                table: "Notification");
        }
    }
}
