using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BreedingRequestStatuscoverttoEnumnotBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInBreedingPeriod",
                table: "Pet_Dogs");

            migrationBuilder.DropColumn(
                name: "IsInBreedingPeriod",
                table: "Pet_Cats");

            migrationBuilder.AddColumn<int>(
                name: "breedingRequestStatus",
                table: "Pet_Dogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "breedingRequestStatus",
                table: "Pet_Cats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "breedingRequestStatus",
                table: "Pet_Dogs");

            migrationBuilder.DropColumn(
                name: "breedingRequestStatus",
                table: "Pet_Cats");

            migrationBuilder.AddColumn<bool>(
                name: "IsInBreedingPeriod",
                table: "Pet_Dogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInBreedingPeriod",
                table: "Pet_Cats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
