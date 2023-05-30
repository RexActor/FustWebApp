using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FustWebApp.Migrations
{
    public partial class AdjustmentsV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "adjustmentDate",
                table: "Adjustments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adjustmentDate",
                table: "Adjustments");
        }
    }
}
