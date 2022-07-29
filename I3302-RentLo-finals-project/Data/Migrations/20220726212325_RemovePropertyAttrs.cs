using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace I3302_RentLo_finals_project.Migrations
{
    public partial class RemovePropertyAttrs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorites",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "MaxGuests",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "NumberOfBathrooms",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "NumberOfBeds",
                table: "Property");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Favorites",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxGuests",
                table: "Property",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBathrooms",
                table: "Property",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBeds",
                table: "Property",
                type: "int",
                nullable: true);
        }
    }
}
