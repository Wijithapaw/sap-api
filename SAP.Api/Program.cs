using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SAP.Data;
using SAP.Domain.Constants;
using SAP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAP.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            SeedDataAsync(host).Wait();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task SeedDataAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<SapDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();

                await dbContext.Database.MigrateAsync();

                if(!dbContext.Users.Any())
                {
                    //Create admin user
                    var admin = new User
                    {
                        FirstName = "Wijitha",
                        LastName = "Wijenayake",
                        Email = "wijithapaw@gmail.com",                        
                        UserName = "wijithapaw@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        EmailConfirmed = true,
                    };
                   
                    await userManager.CreateAsync(admin, "User@123");

                    //Create Roles
                    var adminRole = new Role { Id = "role-admin", Name = IdentityRoles.Admin };
                    var pmRole = new Role { Id = "role-project-manager", Name = IdentityRoles.ProjectManager };
                    var analyzerRole = new Role { Id = "role-data-analyzer", Name = IdentityRoles.DataAnalyzer };

                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(pmRole);
                    await roleManager.CreateAsync(analyzerRole);

                    //Add basic claims to roles
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.SapPermission, CustomClaims.FinancialReports));
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.SapPermission, CustomClaims.LookupsFullAccess));
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.SapPermission, CustomClaims.ProjectsFullAccess));
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.SapPermission, CustomClaims.TransactionEntry));

                    await roleManager.AddClaimAsync(pmRole, new Claim(CustomClaimTypes.SapPermission, CustomClaims.FinancialReports));
                    await roleManager.AddClaimAsync(pmRole, new Claim(CustomClaimTypes.SapPermission, CustomClaims.TransactionEntry));

                    await roleManager.AddClaimAsync(analyzerRole, new Claim(CustomClaimTypes.SapPermission, CustomClaims.FinancialReports));

                    //Add Admin role to admin user
                    await userManager.AddToRoleAsync(admin, IdentityRoles.Admin);
                }
            }
        }
    }
}
