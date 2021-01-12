using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetAndPlay.Core.Migrations
{
    public partial class OfferPeriodsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFreePeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsEveryday = table.Column<bool>(nullable: false),
                    IsDayOfWeek = table.Column<bool>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    HoursFrom = table.Column<int>(nullable: false),
                    HoursTo = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFreePeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFreePeriods_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOfferPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsEveryday = table.Column<bool>(nullable: false),
                    IsDayOfWeek = table.Column<bool>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    HoursFrom = table.Column<int>(nullable: false),
                    HoursTo = table.Column<int>(nullable: false),
                    UserOfferId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOfferPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOfferPeriods_UserOffers_UserOfferId",
                        column: x => x.UserOfferId,
                        principalTable: "UserOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFreePeriods_UserId",
                table: "UserFreePeriods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOfferPeriods_UserOfferId",
                table: "UserOfferPeriods",
                column: "UserOfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFreePeriods");

            migrationBuilder.DropTable(
                name: "UserOfferPeriods");
        }
    }
}
