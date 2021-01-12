using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetAndPlay.Core.Migrations
{
    public partial class AddPlaceTypeToOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlaceType",
                table: "UserOffers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaceType",
                table: "UserOffers");
        }
    }
}
