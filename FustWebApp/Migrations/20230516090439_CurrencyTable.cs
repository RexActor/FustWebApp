using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FustWebApp.Migrations
{
    public partial class CurrencyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    currencyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    currencyName = table.Column<string>(type: "TEXT", nullable: false),
                    currencyAbrevation = table.Column<string>(type: "TEXT", nullable: false),
                    currencyTargetName = table.Column<string>(type: "TEXT", nullable: false),
                    currencyTargetNameAbrevation = table.Column<string>(type: "TEXT", nullable: false),
                    currencyExchangeRate = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.currencyId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
