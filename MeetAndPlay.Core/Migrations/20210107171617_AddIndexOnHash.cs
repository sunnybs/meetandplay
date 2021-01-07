using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetAndPlay.Core.Migrations
{
    public partial class AddIndexOnHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Files_Hash",
                table: "Files",
                column: "Hash",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_Hash",
                table: "Files");
        }
    }
}
