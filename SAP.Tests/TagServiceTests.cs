using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Services;
using SAP.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SAP.Tests
{
    public class TagServiceTests
    {
        public class Search
        {
            [Theory]
            [InlineData("", 4)]
            [InlineData(null, 4)]
            [InlineData("infra", 1)]
            [InlineData("cultivation", 1)]
            [InlineData("cultivation-x", 0)]
            public async Task SearchTags_BasedOnTheSearchTerm(string searchTerm, int count)
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var tags = await service.SearchAsync(searchTerm);

                       Assert.Equal(count, tags.Count);
                   });
            }
        }

        public class Create
        {
            [Fact]
            public async Task WhenCreatingaTag_CreatedSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var id = await service.CreateAsync("tag-10");

                       Assert.NotNull(id);
                   });
            }

            [Fact]
            public async Task WhenCreating_TagIsSanitized()
            {
                string id = null;
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                        id = await service.CreateAsync("My Tag ");

                       Assert.NotNull(id);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var tag = await dbContext.Tags.FindAsync(id);

                       Assert.Equal("my tag", tag.Name);
                   });
            }

            [Theory]
            [InlineData("cultivation")]
            [InlineData("CULtivation")]
            [InlineData("CULTIVATION")]
            [InlineData("  CULTIVATION  ")]
            public async Task WhenCreatingExistingTag_ThrowsException(string tag)
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       await Assert.ThrowsAsync<DbUpdateException>(() => service.CreateAsync(tag));
                   });
            }
        }

        private static async Task SetupTestDataAsync(IDbContext dbContext)
        {
            dbContext.Tags.AddRange(TestData.Tags.GetTags());

            await dbContext.SaveChangesAsync();
        }

        private static TagService CreateService(IDbContext dbContext)
        {
            var service = new TagService(dbContext);
            return service;
        }
    }
}
