using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP.Data.Migrations
{
    public partial class Create_Tables_Lookups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LookupHeaders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    LastUpdatedDateUtc = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookups",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    LastUpdatedDateUtc = table.Column<DateTime>(nullable: false),
                    HeaderId = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Inactive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lookups_LookupHeaders_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "LookupHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LookupHeaders_Code",
                table: "LookupHeaders",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lookups_HeaderId_Code",
                table: "Lookups",
                columns: new[] { "HeaderId", "Code" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "LookupHeaders");
        }
    }
}
