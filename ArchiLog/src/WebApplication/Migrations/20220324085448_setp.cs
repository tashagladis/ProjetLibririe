using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class setp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userID",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_userID",
                table: "Messages",
                column: "userID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_RegisterModels_userID",
                table: "Messages",
                column: "userID",
                principalTable: "RegisterModels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_RegisterModels_userID",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_userID",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "userID",
                table: "Messages");
        }
    }
}
