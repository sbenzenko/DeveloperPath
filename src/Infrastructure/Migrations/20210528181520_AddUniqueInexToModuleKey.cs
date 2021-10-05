using Microsoft.EntityFrameworkCore.Migrations;

namespace DeveloperPath.Infrastructure.Migrations
{
    public partial class AddUniqueInexToModuleKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Modules SET [KEY] = 'key-'+ CAST(ID AS VARCHAR(10))");

            migrationBuilder.CreateIndex(
                name: "UX_Modules_Key",
                table: "Modules",
                column: "Key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Modules_Key",
                table: "Modules");
        }
    }
}
