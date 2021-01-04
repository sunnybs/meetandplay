using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetAndPlay.Core.Migrations
{
    public partial class AddGameLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameLevel",
                table: "Lobbies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameLevel",
                table: "Lobbies");
        }
    }
}
