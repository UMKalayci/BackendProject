using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AdvertisementDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Advertisements",
                nullable: false,
                defaultValue:1);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Advertisements",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApplied",
                table: "Advertisements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CityId",
                table: "Advertisements",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Citys_CityId",
                table: "Advertisements",
                column: "CityId",
                principalTable: "Citys",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Citys_CityId",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_CityId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsApplied",
                table: "Advertisements");
        }
    }
}
