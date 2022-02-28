using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Enums;
using SAP.Services;
using SAP.Tests.Helpers;
using System;
using System.Collections.Generic;
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
        }

        public class Get
        {
            [Fact]
            public static async Task WhenProjectExists_ReturnsSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var project = await service.GetAsync("p-1");

                       Assert.Equal("Wire Fence",  project.Title);
                       Assert.Equal("Infrastructure - Wire Fence", project.Description);
                       Assert.Equal(ProjectState.Completed, project.State);
                       Assert.Equal(new DateTime(2020, 4, 1), project.StartDate);
                       Assert.Equal(new DateTime(2020, 6, 1), project.EndDate);
                   });
            }

            [Fact]
            public static async Task WhenProjectDoesNotExists_ReturnsNull()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var project = await service.GetAsync("p-invalid");

                       Assert.Null(project);
                   });
            }
        }

        public class Delete
        {
            [Fact]
            public async Task WhenProjectExists_DeleteSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       await service.DeleteAsync("p-1");
                   },
                   async (IDbContext dbContext) =>
                   {
                       var project = await dbContext.Projects.FindAsync("p-1");

                       Assert.Null(project);
                   });
            }

            [Fact]
            public async Task WhenProjectNotsExists_ThrowsException()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       await Assert.ThrowsAsync<ArgumentNullException>(() => service.DeleteAsync("p-invalid"));
                   });
            }
        }

        public class Create
        {
            public static List<object[]> TestData => new List<object[]>
            {
                new object[] { "Cottage", "Family Cottage", new DateTime(2022, 5, 1), null, ProjectState.Pending },
                new object[] { "Bridge", "Family Cottage", new DateTime(2021, 5, 1), new DateTime(2021, 5, 30), ProjectState.Pending },
                new object[] { "Bridge", "Papaw", null, null, ProjectState.Inprogress }
            };

            [Theory]
            [MemberData(nameof(TestData))]
            public async Task WhenPassingValidData_CreatedSuccessfully(string title, string desc, DateTime? sDate, DateTime? eDate, ProjectState state)
            {
                string projectId = null;

                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var project = DtoHelper.CreateProjectDto(null, title, desc, sDate, eDate, state);

                       projectId = await service.CreateAsync(project);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var project = await dbContext.Projects.FindAsync(projectId);

                       Assert.NotNull(project);
                       Assert.Equal(title, project.Title);
                       Assert.Equal(desc, project.Description);
                       Assert.Equal(sDate, project.StartDate);
                       Assert.Equal(eDate, project.EndDate);
                       Assert.Equal(state, project.State);
                   });
            }
        }

        public class Update
        {
            [Fact]
            public async Task WhenPassingValidData_UpdatesSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var project = DtoHelper.CreateProjectDto(null, "Wire Fence - Updated", "Desc Updated", new DateTime(2022, 1, 1), new DateTime(2022, 1, 31), ProjectState.Pending);

                       await service.UpdateAsync("p-1", project);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var project = await dbContext.Projects.FindAsync("p-1");

                       Assert.NotNull(project);
                       Assert.Equal("Wire Fence - Updated", project.Title);
                       Assert.Equal("Desc Updated", project.Description);
                       Assert.Equal(new DateTime(2022, 1, 1), project.StartDate);
                       Assert.Equal(new DateTime(2022, 1, 31), project.EndDate);
                       Assert.Equal(ProjectState.Pending, project.State);
                   });
            }

            [Fact]
            public async Task WhenPassingInvalidId_ThrowsException()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var project = DtoHelper.CreateProjectDto(null, "Wire Fence - Updated", "Desc Updated", new DateTime(2022, 1, 1), new DateTime(2022, 1, 31), ProjectState.Pending);

                       await Assert.ThrowsAsync<ApplicationException>(() => service.UpdateAsync("p-invalid", project));
                   });
            }
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
