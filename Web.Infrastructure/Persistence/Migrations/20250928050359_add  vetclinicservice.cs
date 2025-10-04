using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addvetclinicservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Services",
                table: "VetClinics");

            migrationBuilder.CreateTable(
                name: "VetClinicService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VetClinicId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createdon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedByid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedon = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VetClinicService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VetClinicService_VetClinics_VetClinicId",
                        column: x => x.VetClinicId,
                        principalTable: "VetClinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VetClinicService_VetClinicId",
                table: "VetClinicService",
                column: "VetClinicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VetClinicService");

            migrationBuilder.AddColumn<string>(
                name: "Services",
                table: "VetClinics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
