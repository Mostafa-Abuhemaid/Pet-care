using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddfavoriteVetsfavoritePets2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cat_Data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites");

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "Favorites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VetClinicId",
                table: "Favorites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                columns: new[] { "UserId", "ProductId", "PetId", "VetClinicId" });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PetId",
                table: "Favorites",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ProductId",
                table: "Favorites",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_VetClinicId",
                table: "Favorites",
                column: "VetClinicId");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_VetClinics_VetClinicId",
                table: "Favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_PetId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_ProductId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_VetClinicId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "VetClinicId",
                table: "Favorites");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                columns: new[] { "ProductId", "UserId" });

            migrationBuilder.CreateTable(
                name: "Cat_Data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Breed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Common_Diseases_Prevention = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Common_Vaccinations = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Createdon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Energy_Level = table.Column<int>(type: "int", nullable: false),
                    FemaleAverageSize = table.Column<double>(type: "float", nullable: false),
                    FemaleAverageWeight = table.Column<double>(type: "float", nullable: false),
                    FemaleTemperament = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Good_With = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Grooming_Frequency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Health_Risks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Hypoallergenic = table.Column<bool>(type: "bit", nullable: false),
                    MaleAverageSize = table.Column<double>(type: "float", nullable: false),
                    MaleAverageWeight = table.Column<double>(type: "float", nullable: false),
                    MaleTemperament = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Physical_Characteristics = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Shedding_Level = table.Column<int>(type: "int", nullable: false),
                    Trainability = table.Column<int>(type: "int", nullable: false),
                    UpdatedByid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vaccination_Frequency_ofTiming = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    vocalization = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cat_Data", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");
        }
    }
}
