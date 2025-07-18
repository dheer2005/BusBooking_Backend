using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusBooking.Migrations
{
    /// <inheritdoc />
    public partial class updatedPaymenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RazorpayOrderId",
                table: "payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RazorpayPaymentId",
                table: "payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RazorpaySignature",
                table: "payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RazorpayOrderId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "RazorpayPaymentId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "RazorpaySignature",
                table: "payments");
        }
    }
}
