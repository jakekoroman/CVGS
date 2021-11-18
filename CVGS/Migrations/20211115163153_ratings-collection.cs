using Microsoft.EntityFrameworkCore.Migrations;

namespace CVGS.Migrations
{
    public partial class ratingscollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GameRatings_GameID",
                table: "GameRatings",
                column: "GameID");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRatings_Game_GameID",
                table: "GameRatings",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRatings_Game_GameID",
                table: "GameRatings");

            migrationBuilder.DropIndex(
                name: "IX_GameRatings_GameID",
                table: "GameRatings");
        }
    }
}
