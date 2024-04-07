using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryDDD.Migrations
{
    public partial class RenameUserTableColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username_Value",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Balance_Value",
                table: "Users",
                newName: "Balance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Username_Value");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Users",
                newName: "Balance_Value");
        }
    }
}
