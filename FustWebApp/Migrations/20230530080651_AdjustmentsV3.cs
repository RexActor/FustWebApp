using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FustWebApp.Migrations
{
    public partial class AdjustmentsV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodeDescription",
                table: "Adjustments",
                newName: "Reason");

            migrationBuilder.AddColumn<int>(
                name: "StockHoldingId",
                table: "Adjustments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AdjustmentCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdjustmentCode = table.Column<string>(type: "TEXT", nullable: false),
                    CodeDescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjustmentCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adjustments_StockHoldingId",
                table: "Adjustments",
                column: "StockHoldingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adjustments_StockHolding_StockHoldingId",
                table: "Adjustments",
                column: "StockHoldingId",
                principalTable: "StockHolding",
                principalColumn: "StockHoldingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adjustments_StockHolding_StockHoldingId",
                table: "Adjustments");

            migrationBuilder.DropTable(
                name: "AdjustmentCodes");

            migrationBuilder.DropIndex(
                name: "IX_Adjustments_StockHoldingId",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "StockHoldingId",
                table: "Adjustments");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Adjustments",
                newName: "CodeDescription");
        }
    }
}
