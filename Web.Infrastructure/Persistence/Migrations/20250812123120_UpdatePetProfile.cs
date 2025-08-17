using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePetProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Characteristic",
                table: "Pet_Dogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "height",
                table: "Pet_Dogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Characteristic",
                table: "Pet_Cats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "height",
                table: "Pet_Cats",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Characteristic",
                table: "Pet_Dogs");

            migrationBuilder.DropColumn(
                name: "height",
                table: "Pet_Dogs");

            migrationBuilder.DropColumn(
                name: "Characteristic",
                table: "Pet_Cats");

            migrationBuilder.DropColumn(
                name: "height",
                table: "Pet_Cats");
        }
    }
}
