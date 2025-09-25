using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateVetclinic2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VetClinics_addresses_LocationId",
                table: "VetClinics");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "VetClinics",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_VetClinics_LocationId",
                table: "VetClinics",
                newName: "IX_VetClinics_AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_VetClinics_addresses_AddressId",
                table: "VetClinics",
                column: "AddressId",
                principalTable: "addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VetClinics_addresses_AddressId",
                table: "VetClinics");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "VetClinics",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_VetClinics_AddressId",
                table: "VetClinics",
                newName: "IX_VetClinics_LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_VetClinics_addresses_LocationId",
                table: "VetClinics",
                column: "LocationId",
                principalTable: "addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
