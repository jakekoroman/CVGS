using Microsoft.EntityFrameworkCore.Migrations;

namespace CVGS.Migrations
{
    public partial class Game : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Developer",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "Game",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Developer",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "Game");
        }
    }
}
