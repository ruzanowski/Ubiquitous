using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace U.NotificationService.Infrastracture.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notifications");

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IntegrationEventId = table.Column<Guid>(nullable: false),
                    IntegrationEvent = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification_confirmation",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    User = table.Column<Guid>(nullable: false),
                    NotificationId = table.Column<Guid>(nullable: false),
                    ConfirmationDate = table.Column<DateTime>(nullable: false),
                    ConfirmationType = table.Column<int>(nullable: false),
                    DeviceReceivedId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification_confirmation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_confirmation_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalSchema: "Notifications",
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_confirmation_NotificationId",
                schema: "Notifications",
                table: "Notification_confirmation",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_confirmation_User",
                schema: "Notifications",
                table: "Notification_confirmation",
                column: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification_confirmation",
                schema: "Notifications");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Notifications");
        }
    }
}
