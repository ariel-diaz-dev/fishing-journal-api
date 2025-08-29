using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace domain.Migrations
{
    /// <inheritdoc />
    public partial class AddFishingReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishingReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstHighTide = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SecondHighTide = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstLowTide = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SecondLowTide = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DaytimeTemperature = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    WaterTemperature = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    WindSpeedInMilesPerHour = table.Column<int>(type: "int", nullable: true),
                    WindDirection = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    WeatherConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TripDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishingReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FishingReports_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FishingReports_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FishingReports_AccountId",
                table: "FishingReports",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FishingReports_DeletedDate",
                table: "FishingReports",
                column: "DeletedDate");

            migrationBuilder.CreateIndex(
                name: "IX_FishingReports_LocationId",
                table: "FishingReports",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FishingReports_TripDate",
                table: "FishingReports",
                column: "TripDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FishingReports");
        }
    }
}
