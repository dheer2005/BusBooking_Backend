using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusBooking.Migrations
{
    /// <inheritdoc />
    public partial class updatedBusRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "BusRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "BusRatings");
        }
    }
}
