using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class onze : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FriendID",
                table: "RegisterModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisterModels_FriendID",
                table: "RegisterModels",
                column: "FriendID");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterModels_Friends_FriendID",
                table: "RegisterModels",
                column: "FriendID",
                principalTable: "Friends",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisterModels_Friends_FriendID",
                table: "RegisterModels");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_RegisterModels_FriendID",
                table: "RegisterModels");

            migrationBuilder.DropColumn(
                name: "FriendID",
                table: "RegisterModels");
        }
    }
}
