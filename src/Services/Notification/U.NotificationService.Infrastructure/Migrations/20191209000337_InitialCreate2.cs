using Microsoft.EntityFrameworkCore.Migrations;

namespace U.NotificationService.Infrastructure.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimesSent",
                schema: "Notifications",
                table: "Notification",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesSent",
                schema: "Notifications",
                table: "Notification");
        }
    }
}
