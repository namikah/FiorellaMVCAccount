using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstFiorellaMVC.Migrations
{
    public partial class aboutSectionClassAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aboutUnstyledLists_aboutContexts_AboutContextId",
                table: "aboutUnstyledLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aboutUnstyledLists",
                table: "aboutUnstyledLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aboutContexts",
                table: "aboutContexts");

            migrationBuilder.RenameTable(
                name: "aboutUnstyledLists",
                newName: "AboutUnstyledLists");

            migrationBuilder.RenameTable(
                name: "aboutContexts",
                newName: "AboutContexts");

            migrationBuilder.RenameIndex(
                name: "IX_aboutUnstyledLists_AboutContextId",
                table: "AboutUnstyledLists",
                newName: "IX_AboutUnstyledLists_AboutContextId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutUnstyledLists",
                table: "AboutUnstyledLists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutContexts",
                table: "AboutContexts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutUnstyledLists_AboutContexts_AboutContextId",
                table: "AboutUnstyledLists",
                column: "AboutContextId",
                principalTable: "AboutContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutUnstyledLists_AboutContexts_AboutContextId",
                table: "AboutUnstyledLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutUnstyledLists",
                table: "AboutUnstyledLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutContexts",
                table: "AboutContexts");

            migrationBuilder.RenameTable(
                name: "AboutUnstyledLists",
                newName: "aboutUnstyledLists");

            migrationBuilder.RenameTable(
                name: "AboutContexts",
                newName: "aboutContexts");

            migrationBuilder.RenameIndex(
                name: "IX_AboutUnstyledLists_AboutContextId",
                table: "aboutUnstyledLists",
                newName: "IX_aboutUnstyledLists_AboutContextId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aboutUnstyledLists",
                table: "aboutUnstyledLists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aboutContexts",
                table: "aboutContexts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_aboutUnstyledLists_aboutContexts_AboutContextId",
                table: "aboutUnstyledLists",
                column: "AboutContextId",
                principalTable: "aboutContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
