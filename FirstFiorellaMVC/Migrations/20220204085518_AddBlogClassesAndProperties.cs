using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstFiorellaMVC.Migrations
{
    public partial class AddBlogClassesAndProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_experts_positions_PositionId",
                table: "experts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_subcribes",
                table: "subcribes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_positions",
                table: "positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_experts",
                table: "experts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expertContexts",
                table: "expertContexts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aboutUnstyledLists",
                table: "aboutUnstyledLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aboutImages",
                table: "aboutImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aboutContexts",
                table: "aboutContexts");

            migrationBuilder.RenameTable(
                name: "subcribes",
                newName: "Subcribes");

            migrationBuilder.RenameTable(
                name: "positions",
                newName: "Positions");

            migrationBuilder.RenameTable(
                name: "experts",
                newName: "Experts");

            migrationBuilder.RenameTable(
                name: "expertContexts",
                newName: "ExpertContexts");

            migrationBuilder.RenameTable(
                name: "aboutUnstyledLists",
                newName: "AboutUnstyledLists");

            migrationBuilder.RenameTable(
                name: "aboutImages",
                newName: "AboutImages");

            migrationBuilder.RenameTable(
                name: "aboutContexts",
                newName: "AboutContexts");

            migrationBuilder.RenameIndex(
                name: "IX_experts_PositionId",
                table: "Experts",
                newName: "IX_Experts_PositionId");

            migrationBuilder.RenameColumn(
                name: "Subtitle",
                table: "ExpertContexts",
                newName: "SubTitle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subcribes",
                table: "Subcribes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Positions",
                table: "Positions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experts",
                table: "Experts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertContexts",
                table: "ExpertContexts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutUnstyledLists",
                table: "AboutUnstyledLists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutImages",
                table: "AboutImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutContexts",
                table: "AboutContexts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BlogContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogContexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Experts_Positions_PositionId",
                table: "Experts",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experts_Positions_PositionId",
                table: "Experts");

            migrationBuilder.DropTable(
                name: "BlogContexts");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subcribes",
                table: "Subcribes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Positions",
                table: "Positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experts",
                table: "Experts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertContexts",
                table: "ExpertContexts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutUnstyledLists",
                table: "AboutUnstyledLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutImages",
                table: "AboutImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutContexts",
                table: "AboutContexts");

            migrationBuilder.RenameTable(
                name: "Subcribes",
                newName: "subcribes");

            migrationBuilder.RenameTable(
                name: "Positions",
                newName: "positions");

            migrationBuilder.RenameTable(
                name: "Experts",
                newName: "experts");

            migrationBuilder.RenameTable(
                name: "ExpertContexts",
                newName: "expertContexts");

            migrationBuilder.RenameTable(
                name: "AboutUnstyledLists",
                newName: "aboutUnstyledLists");

            migrationBuilder.RenameTable(
                name: "AboutImages",
                newName: "aboutImages");

            migrationBuilder.RenameTable(
                name: "AboutContexts",
                newName: "aboutContexts");

            migrationBuilder.RenameIndex(
                name: "IX_Experts_PositionId",
                table: "experts",
                newName: "IX_experts_PositionId");

            migrationBuilder.RenameColumn(
                name: "SubTitle",
                table: "expertContexts",
                newName: "Subtitle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_subcribes",
                table: "subcribes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_positions",
                table: "positions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_experts",
                table: "experts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expertContexts",
                table: "expertContexts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aboutUnstyledLists",
                table: "aboutUnstyledLists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aboutImages",
                table: "aboutImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aboutContexts",
                table: "aboutContexts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_experts_positions_PositionId",
                table: "experts",
                column: "PositionId",
                principalTable: "positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
