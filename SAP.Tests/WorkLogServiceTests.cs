using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Dtos;
using SAP.Services;
using SAP.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SAP.Tests
{
    public class WorkLogServiceTests
    {
        public class Create
        {
            [Fact]
            public async Task WhenCreatingaWorkLogs_CreatedSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var worklogDto = DtoHelper.GetWorkLogDto(null, "p-4", new DateTime(2022, 5, 6), "Nihal", "Spraying pesticides", 2000);

                       var id = await service.CreateAsync(worklogDto);

                       Assert.NotNull(id);
                   });
            }

            [Fact]
            public async Task WhenRquiredFieldsAreMissing_ThrowsException()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var worklogDto = DtoHelper.GetWorkLogDto(null, "p-4", new DateTime(2022, 5, 6), null, null, 2000);

                       await Assert.ThrowsAsync<DbUpdateException>(() => service.CreateAsync(worklogDto));
                   });
            }
        }

        public class Get
        {
            [Fact]
            public async Task WhenExists_ReturnsSuccussfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var log = await service.GetAsync("wl-11");

                       Assert.NotNull(log);
                       Assert.Equal("wl-11", log.Id);
                       Assert.Equal("p-1", log.ProjectId);
                       Assert.Equal("Wire Fence", log.ProjectName);
                       Assert.Equal("James", log.LabourName);
                       Assert.Equal("Digging for posts", log.JobDescription);
                       Assert.Equal(2000, log.Wage);
                   });
            }

            [Fact]
            public async Task WhenNotExists_ReturnsNull()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var log = await service.GetAsync("wl-11-invalid");

                       Assert.Null(log);
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

                       var worklogDto = DtoHelper.GetWorkLogDto(null, "p-4", new DateTime(2022, 6, 6), "Alex", "Watering plants", 2500);

                       await service.UpdateAsync("wl-11", worklogDto);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var log = await dbContext.WorkLogs.FindAsync("wl-11");

                       Assert.NotNull(log);
                       Assert.Equal("wl-11", log.Id);
                       Assert.Equal("p-4", log.ProjectId);
                       Assert.Equal("Alex", log.LabourName);
                       Assert.Equal("Watering plants", log.JobDescription);
                       Assert.Equal(2500, log.Wage);
                   });
            }

            [Fact]
            public async Task WhenPassingInValidData_ThrowsException()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var worklogDto = DtoHelper.GetWorkLogDto(null, "p-4", new DateTime(2022, 6, 6), null, null, 2500);

                       await Assert.ThrowsAsync<DbUpdateException>(() => service.UpdateAsync("wl-11", worklogDto));
                   });
            }
        }

        public class Delete
        {
            [Fact]
            public async Task WhenExists_DeleteSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);
                       await service.DeleteAsync("wl-11");
                   },
                   async (IDbContext dbContext) =>
                   {
                       var log = await dbContext.WorkLogs.FindAsync("wl-11");
                       Assert.Null(log);
                   });
            }
        }

        public class Search
        {
            public static List<object[]> TestData => new List<object[]>
            {
                new object[] { "", "*", null, null, 12, 12 },
                new object[] { "", "*", new DateTime(2020,1,1), new DateTime(2020,12,31), 8, 8 },
                new object[] { "", "p-1", null, null, 4, 4 },
                new object[] { "", "p-1", new DateTime(2020, 1, 1), new DateTime(2020, 1, 1), 3, 3 },
                new object[] { "jaMes", "*", null, null, 3, 3 },
                new object[] { "", "*", null, null, 5, 12, 1, 5 },
                new object[] { "", "*", null, null, 5, 12, 2, 5 },
                new object[] { "", "*", null, null, 2, 12, 3, 5 },
                new object[] { "bridge", "*", null, null, 2, 4, 1, 2 },
            };

            [Theory]
            [MemberData(nameof(TestData))]
            public async Task ReturnsLogs_BasedOnFilter(string searchTerm, string projectIds, DateTime? from, DateTime? to, int expectedCount, int expectedTotal, int page = 1, int pageSize=100)
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var dto = new WorkLogSearchDto
                      {
                          SearchTerm = searchTerm,
                          From = from,
                          To = to,
                          Page = page,
                          PageSize = pageSize,
                          Projects = projectIds
                      };
                      var logs = await service.SearchAsync(dto);

                      Assert.Equal(expectedTotal, logs.Total);
                      Assert.Equal(expectedCount, logs.Items.Count);
                  });
            }
        }

        private static async Task SetupTestDataAsync(IDbContext dbContext)
        {
            dbContext.Users.AddRange(TestData.Users.GetUsers());
            dbContext.Projects.AddRange(TestData.Projects.GetProjects());
            dbContext.WorkLogs.AddRange(TestData.WorkLogs.GetWorkLogs());

            await dbContext.SaveChangesAsync();
        }

        private static WorkLogService CreateService(IDbContext dbContext)
        {
            var service = new WorkLogService(dbContext);
            return service;
        }
    }
}
