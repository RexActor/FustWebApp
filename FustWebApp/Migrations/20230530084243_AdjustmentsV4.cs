using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FustWebApp.Migrations
{
    public partial class AdjustmentsV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustmentCode",
                table: "Adjustments");

            migrationBuilder.AddColumn<int>(
                name: "AdjustmentCodeId",
                table: "Adjustments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromQuantity",
                table: "Adjustments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToQuantity",
                table: "Adjustments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Adjustments_AdjustmentCodeId",
                table: "Adjustments",
                column: "AdjustmentCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adjustments_AdjustmentCodes_AdjustmentCodeId",
                table: "Adjustments",
                column: "AdjustmentCodeId",
                principalTable: "AdjustmentCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adjustments_AdjustmentCodes_AdjustmentCodeId",
                table: "Adjustments");

            migrationBuilder.DropIndex(
                name: "IX_Adjustments_AdjustmentCodeId",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "AdjustmentCodeId",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "FromQuantity",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "ToQuantity",
                table: "Adjustments");

            migrationBuilder.AddColumn<string>(
                name: "AdjustmentCode",
                table: "Adjustments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
