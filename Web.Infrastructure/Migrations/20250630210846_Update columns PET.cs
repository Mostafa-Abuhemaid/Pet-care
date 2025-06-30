using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatecolumnsPET : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Pet_Cats");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Pet_Cats",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MedicalCondidtions",
                table: "Pet_Cats",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Pet_Cats");

            migrationBuilder.DropColumn(
                name: "MedicalCondidtions",
                table: "Pet_Cats");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Pet_Cats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
