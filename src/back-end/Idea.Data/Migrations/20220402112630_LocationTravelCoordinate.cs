using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea.Data.Migrations
{
    public partial class LocationTravelCoordinate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TravelCoordinateId",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TravelCoordinateId",
                table: "Locations",
                column: "TravelCoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Coordinates_TravelCoordinateId",
                table: "Locations",
                column: "TravelCoordinateId",
                principalTable: "Coordinates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Coordinates_TravelCoordinateId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_TravelCoordinateId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "TravelCoordinateId",
                table: "Locations");
        }
    }
}
