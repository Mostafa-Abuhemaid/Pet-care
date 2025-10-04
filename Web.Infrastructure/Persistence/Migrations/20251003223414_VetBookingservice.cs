using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class VetBookingservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByid",
                table: "VetClinicService",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "VetClinicService",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "VetClinicService",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Createdon",
                table: "VetClinicService",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "vetBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    VetClinicId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Createdon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedByid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedon = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vetBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vetBookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vetBookings_VetClinics_VetClinicId",
                        column: x => x.VetClinicId,
                        principalTable: "VetClinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vetBookingServices",
                columns: table => new
                {
                    VetBookingId = table.Column<int>(type: "int", nullable: false),
                    VetClinicServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vetBookingServices", x => new { x.VetBookingId, x.VetClinicServiceId });
                    table.ForeignKey(
                        name: "FK_vetBookingServices_VetClinicService_VetClinicServiceId",
                        column: x => x.VetClinicServiceId,
                        principalTable: "VetClinicService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vetBookingServices_vetBookings_VetBookingId",
                        column: x => x.VetBookingId,
                        principalTable: "vetBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VetClinicService_Name",
                table: "VetClinicService",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_VetClinicService_UpdatedByid",
                table: "VetClinicService",
                column: "UpdatedByid");

            migrationBuilder.CreateIndex(
                name: "IX_vetBookings_PetId",
                table: "vetBookings",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_vetBookings_UserId",
                table: "vetBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_vetBookings_VetClinicId",
                table: "vetBookings",
                column: "VetClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_vetBookingServices_VetClinicServiceId",
                table: "vetBookingServices",
                column: "VetClinicServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vetBookingServices");

            migrationBuilder.DropTable(
                name: "vetBookings");

            migrationBuilder.DropIndex(
                name: "IX_VetClinicService_Name",
                table: "VetClinicService");

            migrationBuilder.DropIndex(
                name: "IX_VetClinicService_UpdatedByid",
                table: "VetClinicService");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByid",
                table: "VetClinicService",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "VetClinicService",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "VetClinicService",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Createdon",
                table: "VetClinicService",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Appointments",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
