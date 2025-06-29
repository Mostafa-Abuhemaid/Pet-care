using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeFKCat_datatotestonly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Cats_Cat_Data_Cat_DataId",
                table: "Pet_Cats");

            migrationBuilder.DropIndex(
                name: "IX_Pet_Cats_Cat_DataId",
                table: "Pet_Cats");

            migrationBuilder.DropColumn(
                name: "Cat_DataId",
                table: "Pet_Cats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cat_DataId",
                table: "Pet_Cats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pet_Cats_Cat_DataId",
                table: "Pet_Cats",
                column: "Cat_DataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Cats_Cat_Data_Cat_DataId",
                table: "Pet_Cats",
                column: "Cat_DataId",
                principalTable: "Cat_Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
