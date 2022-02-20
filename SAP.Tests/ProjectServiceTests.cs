using SAP.Domain;
using SAP.Services;
using SAP.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SAP.Tests
{
    public class ProjectServiceTests
    {
        public class Search
        {
            [Theory]
            [InlineData(null, false, 3)]
            [InlineData(null, true, 1)]
            [InlineData("BRIDGE", false, 1)]
            [InlineData("infra", false, 2)]
            public async Task WhenProjectsAvailable_ReturnAsPerFilter(string searchTerm, bool activeOnly, int expectedCount)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDbContext dbContext) =>
                    {
                        await SetupTestDataAsync(dbContext);
                    },
                    async (IDbContext dbContext) =>
                    {
                        var service = CreateService(dbContext);

                        var projects = await service.SearchAsync(searchTerm, activeOnly);

                        Assert.Equal(expectedCount, projects.Count);
                    });
            }

            private static async Task SetupTestDataAsync(IDbContext dbContext)
            {
                dbContext.Projects.AddRange(TestData.Projects.GetProjects());

                await dbContext.SaveChangesAsync();
            }

            private static ProjectService CreateService(IDbContext dbContext)
            {
                var service = new ProjectService(dbContext);
                return service;
            }
        }
    }
}
