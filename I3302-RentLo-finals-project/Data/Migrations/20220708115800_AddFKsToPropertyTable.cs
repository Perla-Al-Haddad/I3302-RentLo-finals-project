using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace I3302_RentLo_finals_project.Migrations
{
    public partial class AddFKsToPropertyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Property_CategoryId",
                table: "Property",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_CityId",
                table: "Property",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Categories_CategoryId",
                table: "Property",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Cities_CityId",
                table: "Property",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_Categories_CategoryId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_Cities_CityId",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "IX_Property_CategoryId",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "IX_Property_CityId",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Property");
        }
    }
}
