using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstFiorellaMVC.Migrations
{
    public partial class ExpertSectionClassesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "AboutUnstyledLists",
                newName: "aboutUnstyledLists");

            migrationBuilder.RenameTable(
                name: "AboutImages",
                newName: "aboutImages");

            migrationBuilder.RenameTable(
                name: "AboutContexts",
                newName: "aboutContexts");

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

            migrationBuilder.CreateTable(
                name: "expertContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expertContexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "experts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_experts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_experts_positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_experts_PositionId",
                table: "experts",
                column: "PositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expertContexts");

            migrationBuilder.DropTable(
                name: "experts");

            migrationBuilder.DropTable(
                name: "positions");

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
                name: "aboutUnstyledLists",
                newName: "AboutUnstyledLists");

            migrationBuilder.RenameTable(
                name: "aboutImages",
                newName: "AboutImages");

            migrationBuilder.RenameTable(
                name: "aboutContexts",
                newName: "AboutContexts");

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
        }
    }
}
