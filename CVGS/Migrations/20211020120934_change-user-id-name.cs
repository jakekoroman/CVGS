using Microsoft.EntityFrameworkCore.Migrations;

namespace CVGS.Migrations
{
    public partial class changeuseridname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_UserId",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Address",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Address_UserId",
                table: "Address",
                newName: "IX_Address_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_UserID",
                table: "Address",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_UserID",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Address",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_UserID",
                table: "Address",
                newName: "IX_Address_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_UserId",
                table: "Address",
                column: "UserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
