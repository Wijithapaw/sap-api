using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP.Data.Migrations
{
    public partial class Alter_Table_Transactions_AddedForeignKeysToAuditCols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedById",
                table: "Transactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_LastUpdatedById",
                table: "Transactions",
                column: "LastUpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_CreatedById",
                table: "Transactions",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_LastUpdatedById",
                table: "Transactions",
                column: "LastUpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_CreatedById",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_LastUpdatedById",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CreatedById",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_LastUpdatedById",
                table: "Transactions");
        }
    }
}
