using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryDDD.Migrations
{
    public partial class AddSessionStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionStatus",
                table: "GameUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionStatus",
                table: "GameUsers");
        }
    }
}
