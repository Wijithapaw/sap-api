using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Constants;
using SAP.Domain.Dtos;
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
            [InlineData(null, false, 4, "u-1", true)]
            [InlineData(null, true, 2, "u-1", true)]
            [InlineData("BRIDGE", false, 1, "u-1", true)]
            [InlineData("infra", false, 2, "u-1", true)]
            [InlineData(null, false, 2, "u-1", false)]
            [InlineData(null, true, 0, "u-1", false)]
            [InlineData("Pap", false, 1, "u-2", false)]
            [InlineData(null, false, 0, "u-3", false)]
            public async Task WhenProjectsAvailable_ReturnAsPerFilter(string searchTerm, bool activeOnly, int expectedCount, string reqCtxUserId, bool hasAllProjectAccess)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDbContext dbContext) =>
                    {
                        await SetupTestDataAsync(dbContext);
                    },
                    async (IDbContext dbContext) =>
                    {
                        var requestContext = DbHelper.GetRequestContext();

                        requestContext.UserId = reqCtxUserId;
                        if (hasAllProjectAccess)
                            requestContext.PermissionClaims = new string[] { CustomClaims.ProjectsFullAccess };

                        var service = CreateService(dbContext, requestContext);

                        var projects = await service.SearchAsync(searchTerm, activeOnly);

                        Assert.Equal(expectedCount, projects.Count);
                    });
            }
        }

        public class GetProjectsListItems
        {
            [Theory]
            [InlineData(null, 4, "u-1", true)]
            [InlineData("BRIDGE", 1, "u-1", true)]
            [InlineData("infra", 2, "u-1", true)]
            [InlineData(null, 2, "u-1", false)]
            [InlineData("Pap", 1, "u-2", false)]
            [InlineData(null, 0, "u-3", false)]
            public async Task WhenProjectsAvailable_ReturnAsPerFilter(string searchTerm, int expectedCount, string reqCtxUserId, bool hasAllProjectAccess)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDbContext dbContext) =>
                    {
                        await SetupTestDataAsync(dbContext);
                    },
                    async (IDbContext dbContext) =>
                    {
                        var requestContext = DbHelper.GetRequestContext();

                        requestContext.UserId = reqCtxUserId;
                        if (hasAllProjectAccess)
                            requestContext.PermissionClaims = new string[] { CustomClaims.ProjectsFullAccess };

                        var service = CreateService(dbContext, requestContext);

                        var projects = await service.GetProjectsListItemsAsync(searchTerm);

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
                       Assert.Equal("u-1", project.ProjectManagerId);
                       Assert.Equal("Wijitha Wijenayake", project.ProjectManager);
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
                new object[] { "Cottage", "Family Cottage", new DateTime(2022, 5, 1), null, ProjectState.Pending, "u-2" },
                new object[] { "Bridge", "Family Cottage", new DateTime(2021, 5, 1), new DateTime(2021, 5, 30), ProjectState.Pending, "u-3" },
                new object[] { "Bridge", "Papaw", null, null, ProjectState.Inprogress, null }
            };

            [Theory]
            [MemberData(nameof(TestData))]
            public async Task WhenPassingValidData_CreatedSuccessfully(string title, string desc, DateTime? sDate, DateTime? eDate, ProjectState state, string pmId)
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

                       var project = DtoHelper.GetProjectDto(null, title, desc, sDate, eDate, state, pmId);

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
                       Assert.Equal(pmId, project.ProjectManagerId);
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

                       var project = DtoHelper.GetProjectDto(null, "Wire Fence - Updated", "Desc Updated", new DateTime(2022, 1, 1), new DateTime(2022, 1, 31), ProjectState.Pending, "u-3");

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
                       Assert.Equal("u-3", project.ProjectManagerId);
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

                       var project = DtoHelper.GetProjectDto(null, "Wire Fence - Updated", "Desc Updated", new DateTime(2022, 1, 1), new DateTime(2022, 1, 31), ProjectState.Pending);

                       await Assert.ThrowsAsync<ApplicationException>(() => service.UpdateAsync("p-invalid", project));
                   });
            }
        }

        public class AddTag
        {
            [Fact]
            public async Task WhenAddingExistingTag_AddsSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       await service.AddTagAsync("p-1", new ListItemDto { Key = "t-1" });
                   },
                   async (IDbContext dbContext) =>
                   {
                       var projectTag = await dbContext.ProjectTags
                       .FirstOrDefaultAsync(pt => pt.ProjectId == "p-1" && pt.TagId == "t-1");

                       Assert.NotNull(projectTag);
                   });
            }

            [Fact]
            public async Task WhenAddingNewTag_CreatesAndAddsSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       await service.AddTagAsync("p-1", new ListItemDto { Value = "My New Tag" });
                   },
                   async (IDbContext dbContext) =>
                   {
                       var projectTag = await dbContext.ProjectTags
                       .FirstOrDefaultAsync(pt => pt.ProjectId == "p-1" && pt.Tag.Name == "my new tag");

                       Assert.NotNull(projectTag);
                   });
            }
        }

        private static async Task SetupTestDataAsync(IDbContext dbContext)
        {
            dbContext.Users.AddRange(TestData.Users.GetUsers());
            dbContext.Projects.AddRange(TestData.Projects.GetProjects());
            dbContext.Tags.AddRange(TestData.Tags.GetTags());

            await dbContext.SaveChangesAsync();
        }

        private static ProjectService CreateService(IDbContext dbContext, IRequestContext requestContext = null)
        {
            requestContext = requestContext ?? DbHelper.GetRequestContext();
            var tagService = new TagService(dbContext);

            var service = new ProjectService(dbContext, tagService, requestContext);
            return service;
        }
    }
}
