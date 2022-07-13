using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace I3302_RentLo_finals_project.Migrations
{
    public partial class FixUserIdFKUserPropertyRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RenterId",
                table: "UserPropertyRents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserPropertyRents_RenterId",
                table: "UserPropertyRents",
                column: "RenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPropertyRents_AspNetUsers_RenterId",
                table: "UserPropertyRents",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPropertyRents_AspNetUsers_RenterId",
                table: "UserPropertyRents");

            migrationBuilder.DropIndex(
                name: "IX_UserPropertyRents_RenterId",
                table: "UserPropertyRents");

            migrationBuilder.DropColumn(
                name: "RenterId",
                table: "UserPropertyRents");
        }
    }
}
