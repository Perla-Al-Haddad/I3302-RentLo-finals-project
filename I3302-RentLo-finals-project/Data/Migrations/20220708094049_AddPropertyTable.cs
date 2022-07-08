using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace I3302_RentLo_finals_project.Migrations
{
    public partial class AddPropertyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerDay = table.Column<double>(type: "float", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxGuests = table.Column<int>(type: "int", nullable: false),
                    NumberOfBeds = table.Column<int>(type: "int", nullable: false),
                    NumberOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    Favorites = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Property_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Property_CreatorId",
                table: "Property",
                column: "CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Property");
        }
    }
}
