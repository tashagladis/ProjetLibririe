using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class huit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "RegisterModelRegisterModel",
                columns: table => new
                {
                    DemandsID = table.Column<int>(type: "int", nullable: false),
                    FriendsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterModelRegisterModel", x => new { x.DemandsID, x.FriendsID });
                    table.ForeignKey(
                        name: "FK_RegisterModelRegisterModel_RegisterModels_DemandsID",
                        column: x => x.DemandsID,
                        principalTable: "RegisterModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisterModelRegisterModel_RegisterModels_FriendsID",
                        column: x => x.FriendsID,
                        principalTable: "RegisterModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisterModelRegisterModel_FriendsID",
                table: "RegisterModelRegisterModel",
                column: "FriendsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterModelRegisterModel");

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
    }
}
