using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace U.FetchService.Infrastructure.Migrations
{
    public partial class IntialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemsCount = table.Column<int>(nullable: false),
                    Executed_At = table.Column<DateTime>(nullable: false),
                    Executed_By = table.Column<string>(nullable: true),
                    Executed_ExecutorComment = table.Column<string>(nullable: true),
                    From_Name = table.Column<string>(nullable: true),
                    From_Ip = table.Column<string>(nullable: true),
                    From_Port = table.Column<int>(nullable: false),
                    From_Protocol = table.Column<string>(nullable: true),
                    To_Name = table.Column<string>(nullable: true),
                    To_Ip = table.Column<string>(nullable: true),
                    To_Port = table.Column<int>(nullable: false),
                    To_Protocol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
