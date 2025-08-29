using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace domain.Migrations
{
    /// <inheritdoc />
    public partial class AddFishSpeciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishSpecies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScientificName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishSpecies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FishSpecies",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "Order", "ScientificName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Highly prized gamefish with distinctive black lateral line. Found in mangroves, inlets, and shallow flats. Slot limit 28-33 inches.", "Common Snook", 1, "Centropomus undecimalis" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The 'Silver King' - massive gamefish known for spectacular jumps. Catch and release only. Found in channels, flats, and backcountry.", "Tarpon", 2, "Megalops atlanticus" },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Copper-bronze fish with distinctive black spots. Excellent table fare and strong fighter. Found on flats and in shallow water.", "Redfish (Red Drum)", 3, "Sciaenops ocellatus" },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Popular inshore gamefish with dark spots. Excellent eating and fun to catch. Found over grass flats and shallow waters.", "Spotted Seatrout", 4, "Cynoscion nebulosus" },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The 'Gray Ghost' of the flats. Extremely wary and challenging to catch. Primarily found on shallow sandy flats in the Keys.", "Bonefish", 5, "Albula vulpes" },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elite gamefish of the flats. Difficult to hook and strong fighter. Found on sandy flats and around wrecks in the Keys.", "Permit", 6, "Trachinotus falcatus" },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Excellent table fare found around mangroves and structure. Gray to reddish coloration with two canine teeth.", "Mangrove Snapper", 7, "Lutjanus griseus" },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aggressive predator and strong fighter. Bronze to yellow coloration. Found in schools throughout South Florida waters.", "Jack Crevalle", 8, "Caranx hippos" },
                    { 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fierce predator with razor-sharp teeth. Excellent for catching on artificial lures. Found around reefs and flats.", "Barracuda", 9, "Sphyraena barracuda" },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Black and white striped fish with human-like teeth. Excellent table fare. Found around structure, docks, and bridges.", "Sheepshead", 10, "Archosargus probatocephalus" },
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Large bottom-dwelling fish with barbels under chin. Found in shallow waters and around oyster bars.", "Black Drum", 11, "Pogonias cromis" },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flatfish that lies on sandy bottoms. Excellent table fare. Both eyes on the same side of head when mature.", "Flounder", 12, "Paralichthys albigutta" },
                    { 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Acrobatic fighter that jumps when hooked. Silver coloration. Found in shallow waters and canals throughout South Florida.", "Ladyfish", 13, "Elops saurus" },
                    { 14, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fast-swimming fish with yellow spots. Good table fare when fresh. Found in nearshore and inshore waters.", "Spanish Mackerel", 14, "Scomberomorus maculatus" },
                    { 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Large brown fish often mistaken for shark. Excellent eating and strong fighter. Often follows rays and sharks.", "Cobia", 15, "Rachycentron canadum" },
                    { 16, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Unique fish that floats on its side mimicking debris. Excellent table fare. Found around floating objects and structure.", "Tripletail", 16, "Lobotes surinamensis" },
                    { 17, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Premium table fare with silvery, laterally compressed body. Found on sandy beaches and flats.", "Pompano", 17, "Trachinotus carolinus" },
                    { 18, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Small jack species often found in schools. Good live bait for larger fish. Blue-green coloration on top.", "Blue Runner", 18, "Caranx crysos" },
                    { 19, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Common baitfish with distinctive dark spot. Found around grass beds and structure. Often used as bait for larger fish.", "Pinfish", 19, "Lagodon rhomboides" },
                    { 20, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Common reef fish that makes grunting sound. Silver with blue and yellow stripes. Found around structure and reefs.", "Grunt", 20, "Haemulon plumieri" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FishSpecies_Name",
                table: "FishSpecies",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FishSpecies_Order",
                table: "FishSpecies",
                column: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FishSpecies");
        }
    }
}
