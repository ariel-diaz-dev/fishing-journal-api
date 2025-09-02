using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace domain.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoUrlToFishingReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "FishingReports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "FishingReports");
        }
    }
}
