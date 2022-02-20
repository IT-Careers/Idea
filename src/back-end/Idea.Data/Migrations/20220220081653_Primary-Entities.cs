using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea.Data.Migrations
{
    public partial class PrimaryEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicChemicals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicChemicals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasicMaterials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplexMaterials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FarmStations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NecessitiesStations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NecessitiesStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NuclearStations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NuclearStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanetaryTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanetaryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingStations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecyclingStations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecyclingStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Capacity = table.Column<double>(type: "float", nullable: false),
                    HullIntegrityPercentage = table.Column<double>(type: "float", nullable: false),
                    FuelPercentage = table.Column<double>(type: "float", nullable: false),
                    FTLCooldown = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    X = table.Column<long>(type: "bigint", nullable: false),
                    Y = table.Column<long>(type: "bigint", nullable: false),
                    Z = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplexMaterialRequirements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ComplexMaterialId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BasicMaterialId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexMaterialRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexMaterialRequirements_BasicMaterials_BasicMaterialId",
                        column: x => x.BasicMaterialId,
                        principalTable: "BasicMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplexMaterialRequirements_ComplexMaterials_ComplexMaterialId",
                        column: x => x.ComplexMaterialId,
                        principalTable: "ComplexMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_LocationTypes_LocationTypeId",
                        column: x => x.LocationTypeId,
                        principalTable: "LocationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planetaries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Atmosphere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHabitable = table.Column<bool>(type: "bit", nullable: false),
                    PlanetaryTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planetaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planetaries_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Planetaries_PlanetaryTypes_PlanetaryTypeId",
                        column: x => x.PlanetaryTypeId,
                        principalTable: "PlanetaryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stars",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stars_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialDeposits",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanetaryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BasicMaterialId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialDeposits_BasicMaterials_BasicMaterialId",
                        column: x => x.BasicMaterialId,
                        principalTable: "BasicMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialDeposits_Planetaries_PlanetaryId",
                        column: x => x.PlanetaryId,
                        principalTable: "Planetaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    X = table.Column<long>(type: "bigint", nullable: false),
                    Y = table.Column<long>(type: "bigint", nullable: false),
                    Z = table.Column<long>(type: "bigint", nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PlanetaryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StarId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coordinates_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Coordinates_Planetaries_PlanetaryId",
                        column: x => x.PlanetaryId,
                        principalTable: "Planetaries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Coordinates_Stars_StarId",
                        column: x => x.StarId,
                        principalTable: "Stars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComplexMaterialRequirements_BasicMaterialId",
                table: "ComplexMaterialRequirements",
                column: "BasicMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplexMaterialRequirements_ComplexMaterialId",
                table: "ComplexMaterialRequirements",
                column: "ComplexMaterialId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationTypeId",
                table: "Locations",
                column: "LocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDeposits_BasicMaterialId",
                table: "MaterialDeposits",
                column: "BasicMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDeposits_PlanetaryId",
                table: "MaterialDeposits",
                column: "PlanetaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Planetaries_LocationId",
                table: "Planetaries",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Planetaries_PlanetaryTypeId",
                table: "Planetaries",
                column: "PlanetaryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_UserId",
                table: "Ships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stars_LocationId",
                table: "Stars",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicChemicals");

            migrationBuilder.DropTable(
                name: "ComplexMaterialRequirements");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropTable(
                name: "FarmStations");

            migrationBuilder.DropTable(
                name: "MaterialDeposits");

            migrationBuilder.DropTable(
                name: "NecessitiesStations");

            migrationBuilder.DropTable(
                name: "NuclearStations");

            migrationBuilder.DropTable(
                name: "ProcessingStations");

            migrationBuilder.DropTable(
                name: "RecyclingStations");

            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "ComplexMaterials");

            migrationBuilder.DropTable(
                name: "Stars");

            migrationBuilder.DropTable(
                name: "BasicMaterials");

            migrationBuilder.DropTable(
                name: "Planetaries");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "PlanetaryTypes");

            migrationBuilder.DropTable(
                name: "LocationTypes");
        }
    }
}
