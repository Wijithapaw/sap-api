using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP.Data.Migrations
{
    public partial class Alter_Table_WorkLogs_AddedNewCol_TransactionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "WorkLogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_TransactionId",
                table: "WorkLogs",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogs_Transactions_TransactionId",
                table: "WorkLogs",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogs_Transactions_TransactionId",
                table: "WorkLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkLogs_TransactionId",
                table: "WorkLogs");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "WorkLogs");
        }
    }
}
