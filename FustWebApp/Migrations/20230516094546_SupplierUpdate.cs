using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FustWebApp.Migrations
{
    public partial class SupplierUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currencyId",
                table: "Suppliers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "currencyId",
                table: "LoadFusts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_currencyId",
                table: "Suppliers",
                column: "currencyId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadFusts_currencyId",
                table: "LoadFusts",
                column: "currencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadFusts_Currency_currencyId",
                table: "LoadFusts",
                column: "currencyId",
                principalTable: "Currency",
                principalColumn: "currencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Currency_currencyId",
                table: "Suppliers",
                column: "currencyId",
                principalTable: "Currency",
                principalColumn: "currencyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadFusts_Currency_currencyId",
                table: "LoadFusts");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Currency_currencyId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_currencyId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_LoadFusts_currencyId",
                table: "LoadFusts");

            migrationBuilder.DropColumn(
                name: "currencyId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "currencyId",
                table: "LoadFusts");
        }
    }
}
