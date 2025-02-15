using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoworkingSpaceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalCosttoBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "total_cost",
                table: "Booking",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_cost",
                table: "Booking");
        }
    }
}
