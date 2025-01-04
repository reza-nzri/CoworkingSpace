using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoworkingSpaceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddroomFKtoBookinganddeleteTotalCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_cost",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "desk_id",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RoomId",
                table: "Booking",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK__Booking__room_id",
                table: "Booking",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "room_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Booking__room_id",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_RoomId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "desk_id",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_cost",
                table: "Booking",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
