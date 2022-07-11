using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace I3302_RentLo_finals_project.Migrations
{
    public partial class FixCityFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_Countryid",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPropertyRents_Property_PropertyId",
                table: "UserPropertyRents");

            migrationBuilder.RenameColumn(
                name: "Countryid",
                table: "Cities",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_Countryid",
                table: "Cities",
                newName: "IX_Cities_CountryId");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "UserPropertyRents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPropertyRents_Property_PropertyId",
                table: "UserPropertyRents",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPropertyRents_Property_PropertyId",
                table: "UserPropertyRents");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Cities",
                newName: "Countryid");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                newName: "IX_Cities_Countryid");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "UserPropertyRents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_Countryid",
                table: "Cities",
                column: "Countryid",
                principalTable: "Countries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPropertyRents_Property_PropertyId",
                table: "UserPropertyRents",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
