using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetAndPlay.Core.Migrations
{
    public partial class AddCompressedCopies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompressedFileId",
                table: "UserImages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompressedFileId",
                table: "PlaceImages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompressedFileId",
                table: "LobbyImages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompressedFileId",
                table: "GameImages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_CompressedFileId",
                table: "UserImages",
                column: "CompressedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceImages_CompressedFileId",
                table: "PlaceImages",
                column: "CompressedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyImages_CompressedFileId",
                table: "LobbyImages",
                column: "CompressedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_GameImages_CompressedFileId",
                table: "GameImages",
                column: "CompressedFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameImages_Files_CompressedFileId",
                table: "GameImages",
                column: "CompressedFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LobbyImages_Files_CompressedFileId",
                table: "LobbyImages",
                column: "CompressedFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceImages_Files_CompressedFileId",
                table: "PlaceImages",
                column: "CompressedFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserImages_Files_CompressedFileId",
                table: "UserImages",
                column: "CompressedFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImages_Files_CompressedFileId",
                table: "GameImages");

            migrationBuilder.DropForeignKey(
                name: "FK_LobbyImages_Files_CompressedFileId",
                table: "LobbyImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceImages_Files_CompressedFileId",
                table: "PlaceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserImages_Files_CompressedFileId",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_UserImages_CompressedFileId",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_PlaceImages_CompressedFileId",
                table: "PlaceImages");

            migrationBuilder.DropIndex(
                name: "IX_LobbyImages_CompressedFileId",
                table: "LobbyImages");

            migrationBuilder.DropIndex(
                name: "IX_GameImages_CompressedFileId",
                table: "GameImages");

            migrationBuilder.DropColumn(
                name: "CompressedFileId",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "CompressedFileId",
                table: "PlaceImages");

            migrationBuilder.DropColumn(
                name: "CompressedFileId",
                table: "LobbyImages");

            migrationBuilder.DropColumn(
                name: "CompressedFileId",
                table: "GameImages");
        }
    }
}
