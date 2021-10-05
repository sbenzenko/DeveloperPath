using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeveloperPath.Infrastructure.Migrations
{
    public partial class AddKeyAndDeletedToModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Modules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Modules",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Modules");
        }
    }
}
