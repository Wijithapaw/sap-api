using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SAP.Data.Migrations
{
    public partial class SeedData_LookupHeaders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("LookupHeaders",
                new string[] { "Id", "Code", "Name", "CreatedBy", "CreatedDateUtc", "LastUpdatedBy", "LastUpdatedDateUtc" },
                new object[,]
                {
                    { Guid.NewGuid().ToString(), "EXPENSE_TYPES", "Expense Types", "seed", DateTime.UtcNow, "seed", DateTime.UtcNow },
                    { Guid.NewGuid().ToString(), "INCOME_TYPES", "Income Types", "seed", DateTime.UtcNow, "seed", DateTime.UtcNow },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("LookupHeaders", "Code", new string[] { "EXPENSE_TYPES", "INCOME_TYPES" });
        }
    }
}
