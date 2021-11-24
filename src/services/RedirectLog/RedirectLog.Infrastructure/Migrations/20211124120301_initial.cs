using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedirectLog.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUrls",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OriginalURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueKey = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Redirections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomUrlId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redirections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Redirections_CustomUrls_CustomUrlId",
                        column: x => x.CustomUrlId,
                        principalTable: "CustomUrls",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Redirections_CustomUrlId",
                table: "Redirections",
                column: "CustomUrlId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Redirections");

            migrationBuilder.DropTable(
                name: "CustomUrls");
        }
    }
}
