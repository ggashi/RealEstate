using Microsoft.EntityFrameworkCore.Migrations;

namespace RealEstate.WebUI.Migrations
{
    public partial class AddIsVisibleOnProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Properties",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Properties");
        }
    }
}
