using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FustWebApp.Migrations
{
    public partial class LoadModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoadSupplier",
                table: "Loads",
                newName: "LoadSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Loads_LoadSupplierId",
                table: "Loads",
                column: "LoadSupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Suppliers_LoadSupplierId",
                table: "Loads",
                column: "LoadSupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Suppliers_LoadSupplierId",
                table: "Loads");

            migrationBuilder.DropIndex(
                name: "IX_Loads_LoadSupplierId",
                table: "Loads");

            migrationBuilder.RenameColumn(
                name: "LoadSupplierId",
                table: "Loads",
                newName: "LoadSupplier");
        }
    }
}
