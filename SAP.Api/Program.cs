using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using SAP.Data;
using SAP.Domain.Constants;
using SAP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace SAP.Api
{
    public class Program
    {
        static Logger _logger;

        public static void Main(string[] args)
        {
            SetEbConfig();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _logger = ConfigureLogger(env);
            
            try
            {
                _logger.Info("SAP API Starting....");

                var host = CreateHostBuilder(args).Build();

                SeedDataAsync(host).Wait();

                host.Run();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unable to start system due to unhandled exception");
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();

        public static Logger ConfigureLogger(string env)
        {
            var configFile = "nlog.config";

            var envLogConfigPath = $"nlog.{env}.config";
            if (File.Exists(envLogConfigPath))
                configFile = envLogConfigPath;

            var logger = LogManager.LoadConfiguration(configFile).GetCurrentClassLogger();

            return logger;
        }

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

        /// <summary>
        /// This method is used to fix an issue AWS Beanstalk where it is unable to 
        /// read environment variables
        /// https://stackoverflow.com/questions/40127703/aws-elastic-beanstalk-environment-variables-in-asp-net-core-1-0/47648283#47648283
        /// </summary>
        private static void SetEbConfig()
        {
            var tempConfigBuilder = new ConfigurationBuilder();

            tempConfigBuilder.AddJsonFile(
                @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration",
                optional: true,
                reloadOnChange: true
            );

            var configuration = tempConfigBuilder.Build();

            var ebEnv =
                configuration.GetSection("iis:env")
                    .GetChildren()
                    .Select(pair => pair.Value.Split(new[] { '=' }, 2))
                    .ToDictionary(keypair => keypair[0], keypair => keypair[1]);

            foreach (var keyVal in ebEnv)
            {
                Environment.SetEnvironmentVariable(keyVal.Key, keyVal.Value);
            }
        }
    }
}
