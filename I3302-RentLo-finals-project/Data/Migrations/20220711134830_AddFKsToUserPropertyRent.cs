using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace I3302_RentLo_finals_project.Migrations
{
    public partial class AddFKsToUserPropertyRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPropertyRents_AspNetUsers_UserId",
                table: "UserPropertyRents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPropertyRents_Property_PropertyId",
                table: "UserPropertyRents");

            migrationBuilder.DropIndex(
                name: "IX_UserPropertyRents_UserId",
                table: "UserPropertyRents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserPropertyRents");

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
                name: "FK_UserPropertyRents_Property_PropertyId",
                table: "UserPropertyRents",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPropertyRents_Property_PropertyId",
                table: "UserPropertyRents");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "UserPropertyRents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserPropertyRents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPropertyRents_UserId",
                table: "UserPropertyRents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPropertyRents_AspNetUsers_UserId",
                table: "UserPropertyRents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPropertyRents_Property_PropertyId",
                table: "UserPropertyRents",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id");
        }
    }
}
