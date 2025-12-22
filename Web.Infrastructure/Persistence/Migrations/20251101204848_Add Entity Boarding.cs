using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityBoarding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardingId",
                table: "VetReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoardingId",
                table: "VetClinicService",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoardingId",
                table: "Pet_Dogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoardingId",
                table: "Pet_Cats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoardingId",
                table: "histories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoardingId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Boarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Createdon = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedByid = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Updatedon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    logoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boarding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boarding_addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VetReviews_BoardingId",
                table: "VetReviews",
                column: "BoardingId");

            migrationBuilder.CreateIndex(
                name: "IX_VetClinicService_BoardingId",
                table: "VetClinicService",
                column: "BoardingId");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_Dogs_BoardingId",
                table: "Pet_Dogs",
                column: "BoardingId");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_Cats_BoardingId",
                table: "Pet_Cats",
                column: "BoardingId");

            migrationBuilder.CreateIndex(
                name: "IX_histories_BoardingId",
                table: "histories",
                column: "BoardingId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BoardingId",
                table: "Appointments",
                column: "BoardingId");

            migrationBuilder.CreateIndex(
                name: "IX_Boarding_AddressId",
                table: "Boarding",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Boarding_Name",
                table: "Boarding",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Boarding_PricePerNight",
                table: "Boarding",
                column: "PricePerNight");

            migrationBuilder.CreateIndex(
                name: "IX_Boarding_UpdatedByid",
                table: "Boarding",
                column: "UpdatedByid");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Boarding_BoardingId",
                table: "Appointments",
                column: "BoardingId",
                principalTable: "Boarding",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_histories_Boarding_BoardingId",
                table: "histories",
                column: "BoardingId",
                principalTable: "Boarding",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Cats_Boarding_BoardingId",
                table: "Pet_Cats",
                column: "BoardingId",
                principalTable: "Boarding",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Dogs_Boarding_BoardingId",
                table: "Pet_Dogs",
                column: "BoardingId",
                principalTable: "Boarding",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VetClinicService_Boarding_BoardingId",
                table: "VetClinicService",
                column: "BoardingId",
                principalTable: "Boarding",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VetReviews_Boarding_BoardingId",
                table: "VetReviews",
                column: "BoardingId",
                principalTable: "Boarding",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Boarding_BoardingId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_histories_Boarding_BoardingId",
                table: "histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Cats_Boarding_BoardingId",
                table: "Pet_Cats");

            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Dogs_Boarding_BoardingId",
                table: "Pet_Dogs");

            migrationBuilder.DropForeignKey(
                name: "FK_VetClinicService_Boarding_BoardingId",
                table: "VetClinicService");

            migrationBuilder.DropForeignKey(
                name: "FK_VetReviews_Boarding_BoardingId",
                table: "VetReviews");

            migrationBuilder.DropTable(
                name: "Boarding");

            migrationBuilder.DropIndex(
                name: "IX_VetReviews_BoardingId",
                table: "VetReviews");

            migrationBuilder.DropIndex(
                name: "IX_VetClinicService_BoardingId",
                table: "VetClinicService");

            migrationBuilder.DropIndex(
                name: "IX_Pet_Dogs_BoardingId",
                table: "Pet_Dogs");

            migrationBuilder.DropIndex(
                name: "IX_Pet_Cats_BoardingId",
                table: "Pet_Cats");

            migrationBuilder.DropIndex(
                name: "IX_histories_BoardingId",
                table: "histories");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_BoardingId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "BoardingId",
                table: "VetReviews");

            migrationBuilder.DropColumn(
                name: "BoardingId",
                table: "VetClinicService");

            migrationBuilder.DropColumn(
                name: "BoardingId",
                table: "Pet_Dogs");

            migrationBuilder.DropColumn(
                name: "BoardingId",
                table: "Pet_Cats");

            migrationBuilder.DropColumn(
                name: "BoardingId",
                table: "histories");

            migrationBuilder.DropColumn(
                name: "BoardingId",
                table: "Appointments");
        }
    }
}
