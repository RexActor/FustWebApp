﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FustWebApp.Migrations
{
    public partial class SupplierUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Currency_currencyId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_currencyId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "currencyId",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Suppliers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Suppliers");

            migrationBuilder.AddColumn<int>(
                name: "currencyId",
                table: "Suppliers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_currencyId",
                table: "Suppliers",
                column: "currencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Currency_currencyId",
                table: "Suppliers",
                column: "currencyId",
                principalTable: "Currency",
                principalColumn: "currencyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
