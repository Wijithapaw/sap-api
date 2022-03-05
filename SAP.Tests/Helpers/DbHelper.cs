using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SAP.Data;
using SAP.Domain;
using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Tests.Helpers
{
    internal class DbHelper
    {
        private static SqliteConnection GetSqliteConnection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            return connection;
        }

        private static DbContextOptions<SapDbContext> GetSqliteContextOptions(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<SapDbContext>()
                   .UseSqlite(connection)
                   .Options;

            using (var context = new SapDbContext(options, GetRequestContext()))
            {
                context.Database.EnsureCreated();
            }

            return options;
        }

        public static IRequestContext GetRequestContext() => new RequestContext { UserId = "unit-test" };

        /// <summary>
        /// This is a helper method to execute the 3 stages (Setting up test data, Test execution, Additinal validations) of a test method
        /// </summary>
        /// <param name="funcSetupTestDataAsync(IDataContext context)"> Set up test data inside this method. </param>
        /// <param name="funcTextExecutionAsync(IDataContext context)"> Place the main tst logic inside here. </param>
        /// <param name="funcValidationsAsync(IDataContext context)"> Use this method for additional validations against a fresh data context. This is optional. </param>
        /// <returns></returns>
        public static async Task ExecuteTestAsync(Func<IDbContext, Task> funcSetupTestDataAsync,
            Func<IDbContext, Task> funcTextExecutionAsync,
            Func<IDbContext, Task> funcValidationsAsync = null)
        {
            using (var connection = GetSqliteConnection())
            {
                var options = GetSqliteContextOptions(connection);

                if (funcSetupTestDataAsync != null)
                {
                    using (IDbContext context = new SapDbContext(options, GetRequestContext()))
                    {
                        await funcSetupTestDataAsync(context);
                    }
                }

                using (IDbContext context = new SapDbContext(options, GetRequestContext()))
                {
                    await funcTextExecutionAsync(context);
                }

                if (funcValidationsAsync != null)
                {
                    using (IDbContext context = new SapDbContext(options, GetRequestContext()))
                    {
                        await funcValidationsAsync(context);
                    }
                }
            }
        }
    }
}
