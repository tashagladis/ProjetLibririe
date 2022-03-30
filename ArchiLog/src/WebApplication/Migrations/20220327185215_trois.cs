using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class trois : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imageBasePath",
                table: "RegisterModels",
                newName: "ImageBasePath");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "RegisterModels",
                newName: "ImageType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageBasePath",
                table: "RegisterModels",
                newName: "imageBasePath");

            migrationBuilder.RenameColumn(
                name: "ImageType",
                table: "RegisterModels",
                newName: "Image");
        }
    }
}
