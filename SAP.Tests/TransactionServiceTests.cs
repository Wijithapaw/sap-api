﻿using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Constants;
using SAP.Domain.Dtos;
using SAP.Domain.Enums;
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
    public class TransactionServiceTests
    {
        public class Create
        {
            public static IEnumerable<object[]> TestData => new List<object[]>
            {
                new object[] { "p-1", TransactionCategory.Expense, "lk-11", -100.00, "Daily Salary", new DateTime(2022,1,1) },
                new object[] { "p-1", TransactionCategory.Income, "lk-22", 100.00, null, new DateTime(2022,1,1) }
            };

            [Theory]
            [MemberData(nameof(TestData))]
            public async Task WhenPassingCorrectData_CreatesSuccessfully(string projectId,
                TransactionCategory category,
                string typeId,
                double amount,
                string description,
                DateTime date)
            {
                string txnId = null;
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var dto = DtoHelper.GetTransactionDto(null, projectId, category, typeId, amount, description, date);

                      txnId = await service.CreateAsync(dto);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var txn = await dbContext.Transactions.FindAsync(txnId);

                      Assert.NotNull(txn);
                      Assert.Equal(projectId, txn.ProjectId);
                      Assert.Equal(description, txn.Description);
                      Assert.Equal(amount, txn.Amount);
                      Assert.Equal(date, txn.Date);
                      Assert.Equal(typeId, txn.TypeId);
                      Assert.False(txn.Reconciled);
                      Assert.Null(txn.ReconciledById);
                  });
            }

            [Fact]
            public async Task WhenHavingPermissionToReconcile_CreateAndReconcileSuccessfully()
            {
                string txnId = null;
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var requestContext = DbHelper.GetRequestContext();

                      requestContext.UserId = "u-1";
                      requestContext.PermissionClaims = new string[] { CustomClaims.TransactionReconcile };

                      var service = CreateService(dbContext, requestContext);

                      var dto = DtoHelper.GetTransactionDto(null, "p-1", TransactionCategory.Income, "lk-22", 100.00, null, new DateTime(2022, 1, 1));

                      txnId = await service.CreateAsync(dto, true);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var txn = await dbContext.Transactions.FindAsync(txnId);

                      Assert.NotNull(txn);
                      Assert.Equal("p-1", txn.ProjectId);
                      Assert.Null(txn.Description);
                      Assert.Equal(100.00, txn.Amount);
                      Assert.Equal(new DateTime(2022, 1, 1), txn.Date);
                      Assert.Equal("lk-22", txn.TypeId);
                      Assert.True(txn.Reconciled);
                      Assert.Equal("u-1", txn.ReconciledById);
                  });
            }

            [Fact]
            public async Task WhenNotHavingPermissionToReconcile_CreateSuccessfullyNotReconcile()
            {
                string txnId = null;
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var requestContext = DbHelper.GetRequestContext();

                      requestContext.UserId = "u-1";

                      var service = CreateService(dbContext, requestContext);

                      var dto = DtoHelper.GetTransactionDto(null, "p-1", TransactionCategory.Income, "lk-22", 100.00, null, new DateTime(2022, 1, 1));

                      txnId = await service.CreateAsync(dto, true);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var txn = await dbContext.Transactions.FindAsync(txnId);

                      Assert.NotNull(txn);
                      Assert.Equal("p-1", txn.ProjectId);
                      Assert.Null(txn.Description);
                      Assert.Equal(100.00, txn.Amount);
                      Assert.Equal(new DateTime(2022, 1, 1), txn.Date);
                      Assert.Equal("lk-22", txn.TypeId);
                      Assert.False(txn.Reconciled);
                      Assert.Null(txn.ReconciledById);
                  });
            }

            public static IEnumerable<object[]> TestData2 => new List<object[]>
            {
                new object[] { "p-1", TransactionCategory.Expense, null, -100.00, "Daily Salary", new DateTime(2022,1,1) },
                new object[] { null, TransactionCategory.Income, "lk-22", 100.00, null, new DateTime(2022,1,1) }
            };

            [Theory]
            [MemberData(nameof(TestData2))]
            public async Task WhenThereAreDataIssues_ThrowsException(string projectId,
                TransactionCategory category,
                string typeId,
                double amount,
                string description,
                DateTime date)
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var dto = DtoHelper.GetTransactionDto(null, projectId, category, typeId, amount, description, date);

                      await Assert.ThrowsAsync<DbUpdateException>(() => service.CreateAsync(dto));
                  });
            }
        }

        public class Update
        {
            [Fact]
            public async Task WhenPassingCorrectData_UpdatesSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var dto = DtoHelper.GetTransactionDto("t-1", "p-2", TransactionCategory.Expense, "lk-11", -100.00, "Daily wage", new DateTime(2022, 2, 1));

                      await service.UpdateAsync("t-1", dto);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var txn = await dbContext.Transactions.FindAsync("t-1");

                      Assert.NotNull(txn);
                      Assert.Equal("p-2", txn.ProjectId);
                      Assert.Equal("Daily wage", txn.Description);
                      Assert.Equal(-100.00, txn.Amount);
                      Assert.Equal(new DateTime(2022, 2, 1), txn.Date);
                      Assert.Equal("lk-11", txn.TypeId);
                      Assert.False(txn.Reconciled);
                      Assert.Null(txn.ReconciledById);
                  });
            }

            public class Reconcile
            {
                [Fact]
                public async Task Reconcile_Successfully()
                {
                    await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var requestContext = DbHelper.GetRequestContext();

                      requestContext.UserId = "u-1";

                      var service = CreateService(dbContext, requestContext);

                      await service.ReconcileAsync("t-2");
                  },
                  async (IDbContext dbContext) =>
                  {
                      var txn = await dbContext.Transactions.FindAsync("t-2");

                      Assert.True(txn.Reconciled);
                      Assert.Equal("u-1", txn.ReconciledById);
                  });
                }

                [Fact]
                public async Task UnReconcile_Successfully()
                {
                    await DbHelper.ExecuteTestAsync(
                    async (IDbContext dbContext) =>
                    {
                        await SetupTestDataAsync(dbContext);
                    },
                    async (IDbContext dbContext) =>
                    {
                        var requestContext = DbHelper.GetRequestContext();

                        requestContext.UserId = "u-1";

                        var service = CreateService(dbContext, requestContext);

                        await service.UnReconcileAsync("t-2");
                    },
                    async (IDbContext dbContext) =>
                    {
                        var txn = await dbContext.Transactions.FindAsync("t-2");

                        Assert.False(txn.Reconciled);
                        Assert.Null(txn.ReconciledById);
                    });
                }
            }
        }

        public class Delete
        {
            [Fact]
            public async Task Reconcile_Successfully()
            {
                await DbHelper.ExecuteTestAsync(
              async (IDbContext dbContext) =>
              {
                  await SetupTestDataAsync(dbContext);
              },
              async (IDbContext dbContext) =>
              {
                  var requestContext = DbHelper.GetRequestContext();

                  requestContext.UserId = "u-1";

                  var service = CreateService(dbContext, requestContext);

                  await service.DeleteAsync("t-2");
              },
              async (IDbContext dbContext) =>
              {
                  var txn = await dbContext.Transactions.FindAsync("t-2");

                  Assert.Null(txn);
              });
            }
        }

        public class Get
        {
            [Fact]
            public async Task WhenExists_ReturnsCorrectData()
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var txn = await service.GetAsync("t-1");

                      Assert.NotNull(txn);
                      Assert.Equal("p-1", txn.ProjectId);
                      Assert.Equal(TransactionCategory.Expense, txn.Category);
                      Assert.Equal("Daily Wage", txn.Description);
                      Assert.Equal(-100.00, txn.Amount);
                      Assert.Equal(new DateTime(2022, 1, 1), txn.Date);
                      Assert.Equal("lk-11", txn.TypeId);
                      Assert.True(txn.Reconciled);
                      Assert.Equal("u-1", txn.ReconciledById);
                  });
            }
        }

        public class Search
        {
            public static List<object[]> TestData => new List<object[]>
            {
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), null, null, null, null, null, new string[] { } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "*", null, null, null, null, new string[] { } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "*,p-1", null, null, null, null, new string[] { "t-1", "t-2", "t-3" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1,p-2,p-3", null, null, null, null, new string[] { "t-1", "t-2", "t-3", "t-4", "t-5", "t-6", "t-7", "t-8", "t-9", "t-10" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1", null, null, null, null, new string[] { "t-1", "t-2", "t-3"} },
                new object[] { new DateTime(2022, 2, 1), new DateTime(2022, 2, 28), "p-1, p-2, p-3", null, null, null, null, new string[] { "t-8" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1, p-2, p-3", TransactionCategory.Income, null, null, null, new string[] { "t-3", "t-5", "t-6", "t-7", "t-8", "t-9", "t-10" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1, p-2, p-3", TransactionCategory.Expense, new string[] { "lk-11", "lk-12" }, null, null, new string[] { "t-1", "t-2" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "*", TransactionCategory.Expense, new string[] { }, null, null, new string[] { } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1, p-2,p-3", TransactionCategory.Expense, new string[] { "lk-11", "lk-12" }, true, null, new string[] { "t-1" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1, p-2,p-3",TransactionCategory.Expense, new string[] { "lk-11", "lk-12" }, false, null, new string[] { "t-2" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1, p-2,p-3",TransactionCategory.Expense, new string[] { "lk-11", "lk-12" }, null, "cement", new string[] { "t-2" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1, p-2,p-3",null, null, null, "coconut", new string[] { "t-3", "t-4", "t-5", "t-6", "t-7", "t-8", "t-9", "t-10" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "p-1, p-2,p-3",null, null, null, "infra", new string[] { "t-1", "t-2", "t-3", "t-4", "t-5", "t-6", "t-7" } },
            };

            [Theory]
            [MemberData(nameof(TestData))]
            public async Task BasedOnFileter_ReturnsTransactions(DateTime from, 
                DateTime to, 
                string projects, 
                TransactionCategory? category, 
                string[] types,
                bool? reconciled,
                string searchTerm,
                string[] expectedTxnIds)
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var filter = new TransactionSearchDto
                      {
                          FromDate = from,
                          ToDate = to,
                          Category = category,
                          Projects = projects,
                          CategotyTypes = types,
                          Reconsiled = reconciled,
                          SearchTerm = searchTerm
                      };

                      var results = await service.SearchAsync(filter);

                      var txnIds = results.Select(r => r.Id).OrderBy(id => id).ToArray();

                      Assert.Equal(expectedTxnIds.OrderBy(id => id), txnIds);
                  });
            }


            public static List<object[]> TestData2 => new List<object[]>
            {
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "*", null, null, null, null, new string[] { "t-1", "t-2", "t-3", "t-4", "t-5", "t-6", "t-7", "t-8", "t-9", "t-10" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "*,p-1", null, null, null, null, new string[] { "t-1", "t-2", "t-3", "t-4", "t-5", "t-6", "t-7", "t-8", "t-9", "t-10" } },
                new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 12), "*", TransactionCategory.Expense, new string[] { "lk-11", "lk-12" }, null, null, new string[] { "t-1", "t-2" } },
            };

            [Theory]
            [MemberData(nameof(TestData2))]
            public async Task WhenThereIsAllProjectAccess_ReturnsTransactions(DateTime from,
                DateTime to,
                string projects,
                TransactionCategory? category,
                string[] types,
                bool? reconciled,
                string searchTerm,
                string[] expectedTxnIds)
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var requestContext = DbHelper.GetRequestContext();

                      requestContext.UserId = "u-1";
                      requestContext.PermissionClaims = new string[] { CustomClaims.ProjectsFullAccess };

                      var service = CreateService(dbContext, requestContext);

                      var filter = new TransactionSearchDto
                      {
                          FromDate = from,
                          ToDate = to,
                          Category = category,
                          Projects = projects,
                          CategotyTypes = types,
                          Reconsiled = reconciled,
                          SearchTerm = searchTerm
                      };

                      var results = await service.SearchAsync(filter);

                      var txnIds = results.Select(r => r.Id).OrderBy(id => id).ToArray();

                      Assert.Equal(expectedTxnIds.OrderBy(id => id), txnIds);
                  });
            }

            [Fact]
            public async Task BasedOnFileter_ReturnsTransactionsWithCorrectData()
            {
                await DbHelper.ExecuteTestAsync(
                  async (IDbContext dbContext) =>
                  {
                      await SetupTestDataAsync(dbContext);
                  },
                  async (IDbContext dbContext) =>
                  {
                      var service = CreateService(dbContext);

                      var filter = new TransactionSearchDto
                      {
                          FromDate = new DateTime(2022, 1, 1),
                          ToDate = new DateTime(2022, 4, 1),
                          Category = null,
                          Projects = "p-1,p-2,p-3",
                          CategotyTypes = null,
                          Reconsiled = null,
                          SearchTerm = null
                      };

                      var results = await service.SearchAsync(filter);

                      var txn = results.FirstOrDefault(tr => tr.Id == "t-1");

                      Assert.NotNull(txn);
                      Assert.Equal("t-1", txn.Id);
                      Assert.Equal("Daily Wage", txn.Description);
                      Assert.Equal(-100, txn.Amount);
                      Assert.Equal("lk-11", txn.TypeId);
                      Assert.Equal("Labour", txn.Type);
                      Assert.Equal("Wire Fence", txn.ProjectName);
                      Assert.Equal(TransactionCategory.Expense, txn.Category);
                      Assert.Equal(new DateTime(2022, 1, 1), txn.Date);
                      Assert.True(txn.Reconciled);
                      Assert.Equal("Wijitha Wijenayake", txn.ReconciledBy);
                  });
            }
        }

        private static async Task SetupTestDataAsync(IDbContext dbContext)
        {
            dbContext.Users.AddRange(TestData.Users.GetUsers());
            dbContext.Projects.AddRange(TestData.Projects.GetProjects());
            dbContext.Tags.AddRange(TestData.Tags.GetTags());
            dbContext.ProjectTags.AddRange(TestData.Projects.GetProjectTags());
            dbContext.LookupHeaders.AddRange(TestData.Lookups.GetLookupHeaders());
            dbContext.Lookups.AddRange(TestData.Lookups.GetLookups());
            dbContext.Transactions.AddRange(TestData.Transactions.GetTransactions());

            await dbContext.SaveChangesAsync();
        }

        private static TransactionService CreateService(IDbContext dbContext, IRequestContext requestContext = null)
        {
            requestContext = requestContext ?? DbHelper.GetRequestContext();
            var tagService = new TagService(dbContext);

            var service = new TransactionService(dbContext, requestContext);
            return service;
        }
    }
}
