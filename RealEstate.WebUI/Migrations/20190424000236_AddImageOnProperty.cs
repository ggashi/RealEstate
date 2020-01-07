using Microsoft.EntityFrameworkCore.Migrations;

namespace RealEstate.WebUI.Migrations
{
    public partial class AddImageOnProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Properties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Properties");
        }
    }
}
