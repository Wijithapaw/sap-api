using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP.Data.Migrations
{
    public partial class Alter_AuditedEntity_RenamedCols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Users",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "Users",
                newName: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Transactions",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "Transactions",
                newName: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Tags",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "Tags",
                newName: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Roles",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "Roles",
                newName: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Projects",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "Projects",
                newName: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Lookups",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "Lookups",
                newName: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "LookupHeaders",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "LookupHeaders",
                newName: "LastUpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                newName: "CreatedBy",
                table: "Users",
                name: "CreatedById");

            migrationBuilder.RenameColumn(
                newName: "LastUpdatedBy",
                table: "Users",
                name: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                newName: "CreatedBy",
                table: "Transactions",
                name: "CreatedById");

            migrationBuilder.RenameColumn(
                newName: "LastUpdatedBy",
                table: "Transactions",
                name: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                newName: "CreatedBy",
                table: "Tags",
                name: "CreatedById");

            migrationBuilder.RenameColumn(
                newName: "LastUpdatedBy",
                table: "Tags",
                name: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                newName: "CreatedBy",
                table: "Roles",
                name: "CreatedById");

            migrationBuilder.RenameColumn(
                newName: "LastUpdatedBy",
                table: "Roles",
                name: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                newName: "CreatedBy",
                table: "Projects",
                name: "CreatedById");

            migrationBuilder.RenameColumn(
                newName: "LastUpdatedBy",
                table: "Projects",
                name: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                newName: "CreatedBy",
                table: "Lookups",
                name: "CreatedById");

            migrationBuilder.RenameColumn(
                newName: "LastUpdatedBy",
                table: "Lookups",
                name: "LastUpdatedById");

            migrationBuilder.RenameColumn(
                newName: "CreatedBy",
                table: "LookupHeaders",
                name: "CreatedById");

            migrationBuilder.RenameColumn(
                newName: "LastUpdatedBy",
                table: "LookupHeaders",
                name: "LastUpdatedById");
        }
    }
}
