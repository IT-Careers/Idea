using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea.Data.Migrations
{
    public partial class Positions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_Locations_LocationId",
                table: "Coordinates");

            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_Planetaries_PlanetaryId",
                table: "Coordinates");

            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_Stars_StarId",
                table: "Coordinates");

            migrationBuilder.DropIndex(
                name: "IX_Coordinates_LocationId",
                table: "Coordinates");

            migrationBuilder.DropIndex(
                name: "IX_Coordinates_PlanetaryId",
                table: "Coordinates");

            migrationBuilder.DropIndex(
                name: "IX_Coordinates_StarId",
                table: "Coordinates");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Coordinates");

            migrationBuilder.DropColumn(
                name: "PlanetaryId",
                table: "Coordinates");

            migrationBuilder.DropColumn(
                name: "StarId",
                table: "Coordinates");

            migrationBuilder.AddColumn<string>(
                name: "PositionId",
                table: "Stars",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PositionId",
                table: "Planetaries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PositionId",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FrontLowerLeftId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FrontUpperLeftId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FrontLowerRightId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FrontUpperRightId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackLowerLeftId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackUpperLeftId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackLowerRightId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackUpperRightId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Coordinates_BackLowerLeftId",
                        column: x => x.BackLowerLeftId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Positions_Coordinates_BackLowerRightId",
                        column: x => x.BackLowerRightId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Positions_Coordinates_BackUpperLeftId",
                        column: x => x.BackUpperLeftId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Positions_Coordinates_BackUpperRightId",
                        column: x => x.BackUpperRightId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Positions_Coordinates_FrontLowerLeftId",
                        column: x => x.FrontLowerLeftId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Positions_Coordinates_FrontLowerRightId",
                        column: x => x.FrontLowerRightId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Positions_Coordinates_FrontUpperLeftId",
                        column: x => x.FrontUpperLeftId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Positions_Coordinates_FrontUpperRightId",
                        column: x => x.FrontUpperRightId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stars_PositionId",
                table: "Stars",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Planetaries_PositionId",
                table: "Planetaries",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PositionId",
                table: "Locations",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_BackLowerLeftId",
                table: "Positions",
                column: "BackLowerLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_BackLowerRightId",
                table: "Positions",
                column: "BackLowerRightId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_BackUpperLeftId",
                table: "Positions",
                column: "BackUpperLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_BackUpperRightId",
                table: "Positions",
                column: "BackUpperRightId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_FrontLowerLeftId",
                table: "Positions",
                column: "FrontLowerLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_FrontLowerRightId",
                table: "Positions",
                column: "FrontLowerRightId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_FrontUpperLeftId",
                table: "Positions",
                column: "FrontUpperLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_FrontUpperRightId",
                table: "Positions",
                column: "FrontUpperRightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Positions_PositionId",
                table: "Locations",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Planetaries_Positions_PositionId",
                table: "Planetaries",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Stars_Positions_PositionId",
                table: "Stars",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Positions_PositionId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Planetaries_Positions_PositionId",
                table: "Planetaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Stars_Positions_PositionId",
                table: "Stars");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Stars_PositionId",
                table: "Stars");

            migrationBuilder.DropIndex(
                name: "IX_Planetaries_PositionId",
                table: "Planetaries");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PositionId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Stars");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Planetaries");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "Coordinates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanetaryId",
                table: "Coordinates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StarId",
                table: "Coordinates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_LocationId",
                table: "Coordinates",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_PlanetaryId",
                table: "Coordinates",
                column: "PlanetaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_StarId",
                table: "Coordinates",
                column: "StarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_Locations_LocationId",
                table: "Coordinates",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_Planetaries_PlanetaryId",
                table: "Coordinates",
                column: "PlanetaryId",
                principalTable: "Planetaries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_Stars_StarId",
                table: "Coordinates",
                column: "StarId",
                principalTable: "Stars",
                principalColumn: "Id");
        }
    }
}
