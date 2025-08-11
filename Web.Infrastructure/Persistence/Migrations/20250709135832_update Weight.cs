using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MedicalCondidtions",
                table: "Pet_Dogs",
                newName: "MedicalConditions");

            migrationBuilder.RenameColumn(
                name: "MedicalCondidtions",
                table: "Pet_Cats",
                newName: "MedicalConditions");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Pet_Dogs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Pet_Cats",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MedicalConditions",
                table: "Pet_Dogs",
                newName: "MedicalCondidtions");

            migrationBuilder.RenameColumn(
                name: "MedicalConditions",
                table: "Pet_Cats",
                newName: "MedicalCondidtions");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Pet_Dogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Pet_Cats",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
