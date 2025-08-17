using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addProductStatstoenhanceperformanceforBestSelles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SalesCount = table.Column<int>(type: "int", nullable: false),
                    Createdon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedByid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedon = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStats_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Deleted",
                table: "Products",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StockQuantity",
                table: "Products",
                column: "StockQuantity");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStats_ProductId",
                table: "ProductStats",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStats");

            migrationBuilder.DropIndex(
                name: "IX_Products_Deleted",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_StockQuantity",
                table: "Products");
        }
    }
}
