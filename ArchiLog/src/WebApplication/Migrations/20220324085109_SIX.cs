using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class SIX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterModelID",
                table: "RegisterModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegisterModels_RegisterModelID",
                table: "RegisterModels",
                column: "RegisterModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterModels_RegisterModels_RegisterModelID",
                table: "RegisterModels",
                column: "RegisterModelID",
                principalTable: "RegisterModels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisterModels_RegisterModels_RegisterModelID",
                table: "RegisterModels");

            migrationBuilder.DropIndex(
                name: "IX_RegisterModels_RegisterModelID",
                table: "RegisterModels");

            migrationBuilder.DropColumn(
                name: "RegisterModelID",
                table: "RegisterModels");
        }
    }
}
