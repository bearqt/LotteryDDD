using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryDDD.Migrations
{
    public partial class AddAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BetAmount",
                table: "Game",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetAmount",
                table: "Game");
        }
    }
}
