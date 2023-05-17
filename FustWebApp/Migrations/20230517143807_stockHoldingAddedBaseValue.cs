using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FustWebApp.Migrations
{
    public partial class stockHoldingAddedBaseValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "baseValue",
                table: "StockHolding",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "baseValue",
                table: "StockHolding");
        }
    }
}
