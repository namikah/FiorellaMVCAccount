using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstFiorellaMVC.Migrations
{
    public partial class DeleteOneToManyRelationFromAboutSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutUnstyledLists_AboutContexts_AboutContextId",
                table: "AboutUnstyledLists");

            migrationBuilder.DropIndex(
                name: "IX_AboutUnstyledLists_AboutContextId",
                table: "AboutUnstyledLists");

            migrationBuilder.DropColumn(
                name: "AboutContextId",
                table: "AboutUnstyledLists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AboutContextId",
                table: "AboutUnstyledLists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AboutUnstyledLists_AboutContextId",
                table: "AboutUnstyledLists",
                column: "AboutContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutUnstyledLists_AboutContexts_AboutContextId",
                table: "AboutUnstyledLists",
                column: "AboutContextId",
                principalTable: "AboutContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
