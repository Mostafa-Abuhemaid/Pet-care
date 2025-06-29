using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class testDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FemaleAverageSize",
                table: "Cat_Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FemaleAverageWeight",
                table: "Cat_Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FemaleTemperament",
                table: "Cat_Data",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "MaleAverageSize",
                table: "Cat_Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaleAverageWeight",
                table: "Cat_Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "MaleTemperament",
                table: "Cat_Data",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FemaleAverageSize",
                table: "Cat_Data");

            migrationBuilder.DropColumn(
                name: "FemaleAverageWeight",
                table: "Cat_Data");

            migrationBuilder.DropColumn(
                name: "FemaleTemperament",
                table: "Cat_Data");

            migrationBuilder.DropColumn(
                name: "MaleAverageSize",
                table: "Cat_Data");

            migrationBuilder.DropColumn(
                name: "MaleAverageWeight",
                table: "Cat_Data");

            migrationBuilder.DropColumn(
                name: "MaleTemperament",
                table: "Cat_Data");
        }
    }
}
