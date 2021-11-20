using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeveloperPath.Infrastructure.Migrations
{
    public partial class PathSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Paths",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Paths");
        }
    }
}
