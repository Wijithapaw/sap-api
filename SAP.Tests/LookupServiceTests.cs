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
    public class LookupServiceTests
    {
        public class GetAllLookupHeader
        {
            [Fact]
            public async Task WhenLookupHeadersExists_ReturnAll()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var headers = await service.GetAllLookupHeadersAsync();

                       Assert.NotNull(headers);

                       Assert.Equal(3, headers.Count);

                       var expenseTypes = headers.FirstOrDefault(h => h.Key == "h-1");

                       Assert.Equal("Expense Types", expenseTypes.Value);
                       Assert.Equal("h-1", expenseTypes.Key);
                   });
            }
        }

        public class Get
        {
            [Fact]
            public async Task WhenPassingCorrectId_ReturnsCorrectData()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var lookup = await service.GetAsync("lk-11");

                       Assert.NotNull(lookup);

                       Assert.Equal("lk-11", lookup.Id);
                       Assert.Equal("h-1", lookup.HeaderId);
                       Assert.Equal("LABOUR", lookup.Code);
                       Assert.Equal("Labour", lookup.Name);
                       Assert.False(lookup.Inactive);
                   });
            }

            [Fact]
            public async Task WhenPassingIncorrectId_ReturnsNull()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var lookup = await service.GetAsync("lk-11-invalid");

                       Assert.Null(lookup);
                   });
            }
        }

        public class Create
        {
            [Theory]
            [InlineData("FUEL", "h-1", "Fuel", false)]
            [InlineData("REFRESHMENTS", "h-1", "Refreshments", true)]
            [InlineData("MR", "h-3", "Mr.", false)]
            public async Task WhenPassingCorrectData_CreatedSuccessfully(string code, string headerId, string name, bool inactive)
            {
                string lookupId = null;

                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var dto = DtoHelper.GetLookupDto(null, headerId, code, name, inactive);

                       lookupId = await service.CreateAsync(dto);

                       Assert.NotNull(lookupId);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var lookup = await dbContext.Lookups.FindAsync(lookupId);

                       Assert.NotNull(lookup);

                       Assert.Equal(lookupId, lookup.Id);
                       Assert.Equal(headerId, lookup.HeaderId);
                       Assert.Equal(code, lookup.Code);
                       Assert.Equal(name, lookup.Name);
                       Assert.Equal(inactive, lookup.Inactive);
                   });
            }

            [Fact]
            public async Task WhenPassingIncorrectHeaderId_ThrowsException()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var dto = DtoHelper.GetLookupDto(null, "h-1-incorrect", "FUEL", "Fuel", false);

                       var ex = await Assert.ThrowsAsync<DbUpdateException>(() => service.CreateAsync(dto));

                       Assert.NotNull(ex);
                   });
            }

            [Fact]
            public async Task WhenWhen_ThrowsException()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var dto = DtoHelper.GetLookupDto(null, "h-1", "LABOUR", "Labour", false);

                       var ex = await Assert.ThrowsAsync<DbUpdateException>(() => service.CreateAsync(dto));

                       Assert.NotNull(ex);
                   });
            }
        }

        public class GetByHeaderId
        {
            [Fact]
            public async Task WhenLookupsExistsUnderHeader_ReturnAll()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var lookups = await service.GetByHeaderIdAsync("h-1");

                       Assert.Equal(4, lookups.Count);

                       var lookup1 = lookups.FirstOrDefault(l => l.Id == "lk-11");

                       Assert.NotNull(lookup1);
                       Assert.Equal("lk-11", lookup1.Id);
                       Assert.Equal("LABOUR", lookup1.Code);
                       Assert.Equal("Labour", lookup1.Name);
                       Assert.False(lookup1.Inactive);
                   });
            }

            [Fact]
            public async Task WhenLookupsDoesNotExistsUnderHeader_ReturnsEmptyList()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var lookups = await service.GetByHeaderIdAsync("h-3");

                       Assert.Empty(lookups);
                   });
            }
        }

        public class GetActiveLookupsAsListItems
        {
            [Fact]
            public async Task WhenLookupsExistsUnderHeader_ReturnAllActive()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var lookups = await service.GetActiveLookupsAsListItemseAsync("EXPENSE_TYPES");

                       Assert.Equal(3, lookups.Count);

                       var lookup1 = lookups.FirstOrDefault(l => l.Key == "lk-11");
                       Assert.Equal("Labour", lookup1.Value);

                       Assert.DoesNotContain(lookups, l => l.Key == "lk-14");
                   });
            }

            [Fact]
            public async Task WhenLookupsDoesNotExistsUnderHeader_ReturnsEmptyList()
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDbContext dbContext) =>
                   {
                       await SetupTestDataAsync(dbContext);
                   },
                   async (IDbContext dbContext) =>
                   {
                       var service = CreateService(dbContext);

                       var lookups = await service.GetActiveLookupsAsListItemseAsync("SALUTATIONS");

                       Assert.Empty(lookups);
                   });
            }
        }

        public class Update
        {
            [Theory]
            [InlineData("lk-11", "LABOUR-1", "Labour-1", false)]
            [InlineData("lk-12", "MATERIAL", "MATERIAL-1", true)]
            public async Task WhenPassingCorrectData_UpdatesSuccessfully(string id, string code, string name, bool inactive)
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var dto = DtoHelper.GetLookupDto(id, null, code, name, inactive);

                      await service.UpdateAsync(id, dto);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var lookup = await dbContext.Lookups.FindAsync(id);

                      Assert.Equal(id, lookup.Id);
                      Assert.Equal(name, lookup.Name);
                      Assert.Equal(code, lookup.Code);
                      Assert.Equal(inactive, lookup.Inactive);
                  });
            }

            [Theory]
            [InlineData("lk-11", "MATERIAL", "Labour-1", false)] //Duplicating exisitng code
            [InlineData("lk-12", null, "MATERIAL-1", true)]
            [InlineData("lk-12", "MATERIAL", null, true)]
            public async Task WhenHavingDataIntegrityIssues_ThrowsException(string id, string code, string name, bool inactive)
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var dto = DtoHelper.GetLookupDto(id, null, code, name, inactive);

                      var ex = await Assert.ThrowsAsync<DbUpdateException>(() => service.UpdateAsync(id, dto));
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

                      await service.DeleteAsync("lk-11");
                  },
                  async (IDbContext dbContext) =>
                  {
                      var lookup = await dbContext.Lookups.FindAsync("lk-11");

                      Assert.Null(lookup);
                  });
            }
        }

        private static async Task SetupTestDataAsync(IDbContext dbContext)
        {
            dbContext.LookupHeaders.AddRange(TestData.Lookups.GetLookupHeaders());
            dbContext.Lookups.AddRange(TestData.Lookups.GetLookups());

            await dbContext.SaveChangesAsync();
        }

        private static LookupService CreateService(IDbContext dbContext)
        {
            var service = new LookupService(dbContext);
            return service;
        }
    }
}
