using Microsoft.EntityFrameworkCore.Migrations;

namespace JapTask1.Database.Migrations
{
    public partial class InitialMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsageCount",
                table: "Recipes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsageCount",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
