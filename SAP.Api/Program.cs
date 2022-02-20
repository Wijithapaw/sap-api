using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SAP.Data;
using SAP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

                dbContext.Database.EnsureDeleted();

                await dbContext.Database.MigrateAsync();

                if(!dbContext.Users.Any())
                {
                    var adminRole = new Role { Name = "Admin" };
                    var accountantRole = new Role { Name = "Accountant" };
                    var analyzerRole = new Role { Name = "Analyzer" };

                    var admin = new User
                    {
                        Email = "wijithapaw@gmail.com",                        
                        UserName = "wijithapaw@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        EmailConfirmed = true,
                    };

                    await userManager.CreateAsync(admin, "User@123");

                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(accountantRole);
                    await roleManager.CreateAsync(analyzerRole);

                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
