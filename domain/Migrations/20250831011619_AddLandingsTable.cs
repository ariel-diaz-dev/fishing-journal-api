using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace domain.Migrations
{
    /// <inheritdoc />
    public partial class AddLandingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Landings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FishSpeciesId = table.Column<int>(type: "int", nullable: false),
                    FishingReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthInInches = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    LureUsed = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RodUsed = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReelUsed = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MainLineTestInPounds = table.Column<int>(type: "int", nullable: true),
                    LeaderLineTestInPounds = table.Column<int>(type: "int", nullable: true),
                    TimeOfCatch = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Released = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Landings_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Landings_FishSpecies_FishSpeciesId",
                        column: x => x.FishSpeciesId,
                        principalTable: "FishSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Landings_FishingReports_FishingReportId",
                        column: x => x.FishingReportId,
                        principalTable: "FishingReports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Landings_Tackle_LureUsed",
                        column: x => x.LureUsed,
                        principalTable: "Tackle",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Landings_Tackle_ReelUsed",
                        column: x => x.ReelUsed,
                        principalTable: "Tackle",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Landings_Tackle_RodUsed",
                        column: x => x.RodUsed,
                        principalTable: "Tackle",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Landings_AccountId",
                table: "Landings",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_DeletedDate",
                table: "Landings",
                column: "DeletedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_FishingReportId",
                table: "Landings",
                column: "FishingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_FishSpeciesId",
                table: "Landings",
                column: "FishSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_LengthInInches",
                table: "Landings",
                column: "LengthInInches");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_LureUsed",
                table: "Landings",
                column: "LureUsed");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_ReelUsed",
                table: "Landings",
                column: "ReelUsed");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_RodUsed",
                table: "Landings",
                column: "RodUsed");

            migrationBuilder.CreateIndex(
                name: "IX_Landings_TimeOfCatch",
                table: "Landings",
                column: "TimeOfCatch");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Landings");
        }
    }
}
