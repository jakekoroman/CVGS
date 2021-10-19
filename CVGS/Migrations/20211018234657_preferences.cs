using Microsoft.EntityFrameworkCore.Migrations;

namespace CVGS.Migrations
{
    public partial class preferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavoriteGameCategory",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoritePlatform",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteGameCategory",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FavoritePlatform",
                table: "User");
        }
    }
}
