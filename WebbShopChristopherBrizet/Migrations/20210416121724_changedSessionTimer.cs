using Microsoft.EntityFrameworkCore.Migrations;

namespace WebbShopChristopherBrizet.Migrations
{
    public partial class changedSessionTimer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionTimer",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionTimer",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
