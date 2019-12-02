using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace U.SubscriptionService.Persistance.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Subscriptions");

            migrationBuilder.CreateTable(
                name: "UserSubscription",
                schema: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Preferences_NumberOfWelcomeMessages = table.Column<int>(nullable: false),
                    Preferences_DoNotNotifyAnyoneAboutMyActivity = table.Column<bool>(nullable: false),
                    Preferences_OrderByCreationTimeDescending = table.Column<bool>(nullable: false),
                    Preferences_OrderByImportancyDescending = table.Column<bool>(nullable: false),
                    Preferences_SeeReadNotifications = table.Column<bool>(nullable: false),
                    Preferences_SeeUnreadNotifications = table.Column<bool>(nullable: false),
                    Preferences_MinimalImportancyLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllowedEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Allowed = table.Column<int>(nullable: false),
                    UserSubscriptionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllowedEvents_UserSubscription_UserSubscriptionId",
                        column: x => x.UserSubscriptionId,
                        principalSchema: "Subscriptions",
                        principalTable: "UserSubscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SignalRConnections",
                schema: "Subscriptions",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ConnectionId = table.Column<string>(nullable: false),
                    UserSubscriptionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignalRConnections", x => new { x.UserId, x.ConnectionId });
                    table.ForeignKey(
                        name: "FK_SignalRConnections_UserSubscription_UserSubscriptionId",
                        column: x => x.UserSubscriptionId,
                        principalSchema: "Subscriptions",
                        principalTable: "UserSubscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowedEvents_UserSubscriptionId",
                table: "AllowedEvents",
                column: "UserSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SignalRConnections_UserSubscriptionId",
                schema: "Subscriptions",
                table: "SignalRConnections",
                column: "UserSubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedEvents");

            migrationBuilder.DropTable(
                name: "SignalRConnections",
                schema: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserSubscription",
                schema: "Subscriptions");
        }
    }
}
