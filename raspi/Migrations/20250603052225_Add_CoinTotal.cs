using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace raspi.Migrations
{
    /// <inheritdoc />
    public partial class Add_CoinTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoinTotal",
                table: "SlotFingerprints",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoinTotal",
                table: "SlotFingerprints");
        }
    }
}
