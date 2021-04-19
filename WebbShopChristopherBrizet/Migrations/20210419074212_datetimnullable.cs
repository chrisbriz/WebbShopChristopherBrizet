using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebbShopChristopherBrizet.Migrations
{
    public partial class datetimnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionTimer2",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "SessionTimer",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionTimer",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "SessionTimer2",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
