using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addReceiptNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptNumber",
                table: "vetBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                table: "vetBookings");
        }
    }
}
