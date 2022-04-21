using Microsoft.EntityFrameworkCore.Migrations;
using SAP.Domain.Constants;

namespace SAP.Data.Migrations
{
    public partial class SeedData_RoleClaims_TransactionDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("RoleClaims",
                new string[] { "RoleId", "ClaimType", "ClaimValue" },
                new object[,]
                {
                    { "role-admin", CustomClaimTypes.SapPermission, CustomClaims.TransactionDelete },
                    { "role-admin", CustomClaimTypes.SapPermission, CustomClaims.TransactionReconcile }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("RoleClaims", "ClaimValue", new string[] { CustomClaims.TransactionDelete, CustomClaims.TransactionReconcile });
        }
    }
}
