using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetAndPlay.Core.Migrations
{
    public partial class AddLastCredentialsChangeDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastCredentialsChangeDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCredentialsChangeDate",
                table: "AspNetUsers");
        }
    }
}
