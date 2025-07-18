using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusBooking.Migrations
{
    /// <inheritdoc />
    public partial class addedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_schedules_ScheduleId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_schedules_ScheduleId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_schedules_Buses_BusId",
                table: "schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_schedules_locations_FromLocationId",
                table: "schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_schedules_locations_ToLocationId",
                table: "schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_schedules",
                table: "schedules");

            migrationBuilder.RenameTable(
                name: "schedules",
                newName: "Schedules");

            migrationBuilder.RenameIndex(
                name: "IX_schedules_ToLocationId",
                table: "Schedules",
                newName: "IX_Schedules_ToLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_schedules_FromLocationId",
                table: "Schedules",
                newName: "IX_Schedules_FromLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_schedules_BusId",
                table: "Schedules",
                newName: "IX_Schedules_BusId");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Schedules_ScheduleId",
                table: "Bookings",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Schedules_ScheduleId",
                table: "Feedbacks",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Buses_BusId",
                table: "Schedules",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_locations_FromLocationId",
                table: "Schedules",
                column: "FromLocationId",
                principalTable: "locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_locations_ToLocationId",
                table: "Schedules",
                column: "ToLocationId",
                principalTable: "locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Schedules_ScheduleId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Schedules_ScheduleId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Buses_BusId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_locations_FromLocationId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_locations_ToLocationId",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "schedules");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_ToLocationId",
                table: "schedules",
                newName: "IX_schedules_ToLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_FromLocationId",
                table: "schedules",
                newName: "IX_schedules_FromLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_BusId",
                table: "schedules",
                newName: "IX_schedules_BusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_schedules",
                table: "schedules",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_schedules_ScheduleId",
                table: "Bookings",
                column: "ScheduleId",
                principalTable: "schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_schedules_ScheduleId",
                table: "Feedbacks",
                column: "ScheduleId",
                principalTable: "schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_schedules_Buses_BusId",
                table: "schedules",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_schedules_locations_FromLocationId",
                table: "schedules",
                column: "FromLocationId",
                principalTable: "locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_schedules_locations_ToLocationId",
                table: "schedules",
                column: "ToLocationId",
                principalTable: "locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
