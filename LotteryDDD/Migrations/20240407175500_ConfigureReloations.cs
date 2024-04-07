using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryDDD.Migrations
{
    public partial class ConfigureReloations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_GameUsers_Games_GameId",
                table: "GameUsers",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameUsers_Games_GameId",
                table: "GameUsers");
        }
    }
}
