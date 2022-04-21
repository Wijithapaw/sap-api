using Microsoft.EntityFrameworkCore.Migrations;
using SAP.Domain.Constants;

namespace SAP.Data.Migrations
{
    public partial class SeedData_UserManage_Claim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("RoleClaims",
                new string[] { "RoleId", "ClaimType", "ClaimValue" },
                new object[,]
                {
                    { "role-admin", CustomClaimTypes.SapPermission, CustomClaims.UsersFullAccess }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("RoleClaims", "ClaimValue", CustomClaims.UsersFullAccess);
        }
    }
}
