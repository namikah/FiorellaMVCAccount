using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstFiorellaMVC.Migrations
{
    public partial class menuIdChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Menus_MenusId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_MenusId",
                table: "Menus");

            migrationBuilder.RenameColumn(
                name: "MenusId",
                table: "Menus",
                newName: "MenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MenuId",
                table: "Menus",
                newName: "MenusId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenusId",
                table: "Menus",
                column: "MenusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Menus_MenusId",
                table: "Menus",
                column: "MenusId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
