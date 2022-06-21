using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameChooserDiscord.Migrations
{
    public partial class Fruit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fruits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TimesChosen = table.Column<int>(type: "INTEGER", nullable: false),
                    TimesDrawn = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fruits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimesDrawn = table.Column<int>(type: "INTEGER", nullable: false),
                    TimesChosen = table.Column<int>(type: "INTEGER", nullable: false),
                    TimesUnheardOf = table.Column<int>(type: "INTEGER", nullable: true),
                    WikipediaUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fruits");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
