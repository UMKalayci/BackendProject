using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class OrganisationDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerAdvertisementComplateds_AdvertisementVolunteers_AdvertisementVolunteerId",
                table: "VolunteerAdvertisementComplateds");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AdvertisementVolunteers_Id",
                table: "AdvertisementVolunteers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertisementVolunteers",
                table: "AdvertisementVolunteers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertisementVolunteers",
                table: "AdvertisementVolunteers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementVolunteers_VolunteerId",
                table: "AdvertisementVolunteers",
                column: "VolunteerId");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerAdvertisementComplateds_AdvertisementVolunteers_AdvertisementVolunteerId",
                table: "VolunteerAdvertisementComplateds",
                column: "AdvertisementVolunteerId",
                principalTable: "AdvertisementVolunteers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerAdvertisementComplateds_AdvertisementVolunteers_AdvertisementVolunteerId",
                table: "VolunteerAdvertisementComplateds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertisementVolunteers",
                table: "AdvertisementVolunteers");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisementVolunteers_VolunteerId",
                table: "AdvertisementVolunteers");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AdvertisementVolunteers_Id",
                table: "AdvertisementVolunteers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertisementVolunteers",
                table: "AdvertisementVolunteers",
                columns: new[] { "VolunteerId", "AdvertisementId" });

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerAdvertisementComplateds_AdvertisementVolunteers_AdvertisementVolunteerId",
                table: "VolunteerAdvertisementComplateds",
                column: "AdvertisementVolunteerId",
                principalTable: "AdvertisementVolunteers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
