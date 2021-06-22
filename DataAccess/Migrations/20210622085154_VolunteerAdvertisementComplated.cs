using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class VolunteerAdvertisementComplated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AdvertisementVolunteers",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AdvertisementVolunteers_Id",
                table: "AdvertisementVolunteers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "VolunteerAdvertisementComplateds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    AdvertisementVolunteerId = table.Column<int>(nullable: false),
                    TotalWork = table.Column<int>(nullable: false),
                    ConfirmationStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerAdvertisementComplateds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerAdvertisementComplateds_AdvertisementVolunteers_AdvertisementVolunteerId",
                        column: x => x.AdvertisementVolunteerId,
                        principalTable: "AdvertisementVolunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerAdvertisementComplateds_AdvertisementVolunteerId",
                table: "VolunteerAdvertisementComplateds",
                column: "AdvertisementVolunteerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VolunteerAdvertisementComplateds");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AdvertisementVolunteers_Id",
                table: "AdvertisementVolunteers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AdvertisementVolunteers");
        }
    }
}
