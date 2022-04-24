using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Constants;
using SAP.Domain.Dtos;
using SAP.Domain.Entities;
using SAP.Domain.Enums;
using SAP.Domain.Exceptions;
using SAP.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAP.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IDbContext _dbContext;
        private readonly IRequestContext _requestContext;

        public TransactionService(IDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }

        public async Task<string> CreateAsync(TransactionDto dto)
        {
            var txn = new Transaction
            {
                Category = dto.Category,
                TypeId = dto.TypeId,
                Amount = dto.Category == TransactionCategory.Expense ? Math.Abs(dto.Amount) * -1 : Math.Abs(dto.Amount),
                Description = dto.Description,
                Date = dto.Date,
                ProjectId = dto.ProjectId,
            };

            if(dto.Reconciled && _requestContext.HasPermission(CustomClaims.TransactionReconcile))
            {
                txn.Reconciled = true;
                txn.ReconciledById = _requestContext.UserId;
            }

            _dbContext.Transactions.Add(txn);

            await _dbContext.SaveChangesAsync();

            return txn.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var txn = await _dbContext.Transactions.FindAsync(id);

            if (txn.Reconciled)
                throw new SapException("ERR_CANT_DELETE_RECONCILED_TXN");

            if (_requestContext.HasPermission(CustomClaims.TransactionDelete) || _requestContext.UserId == txn.CreatedById)
            {
                _dbContext.Transactions.Remove(txn);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new SapException("ERR_INSUFFICIENT_PERMISSION_TO_DELETE_TRNASACTION");
            }
        }

        public async Task<TransactionDto> GetAsync(string id)
        {
            var txn = await _dbContext.Transactions
                .Where(t => t.Id == id)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Description = t.Description,
                    Date = t.Date,
                    Category = t.Category,
                    TypeId = t.TypeId,
                    Type = t.Type.Name,
                    TypeCode = t.Type.Code,
                    ProjectId = t.ProjectId,
                    Reconciled = t.Reconciled,
                    ReconciledById = t.ReconciledById,
                    ReconciledBy = $"{t.ReconciledBy.FirstName} {t.ReconciledBy.LastName}",
                    ReconciledDateUtc = t.ReconciledDateUtc,
                    CreatedBy = $"{t.CreatedBy.FirstName} {t.CreatedBy.LastName}",
                    LastUpdatedBy = $"{t.LastUpdatedBy.FirstName} {t.LastUpdatedBy.LastName}",
                    CreatedDateUtc = t.CreatedDateUtc,
                    LastUpdatedDateUtc = t.LastUpdatedDateUtc,
                }).FirstOrDefaultAsync();

            return txn;
        }

        public async Task ReconcileAsync(string id)
        {
            var txn = await _dbContext.Transactions.FindAsync(id);
            
            txn.Reconciled = true;
            txn.ReconciledById = _requestContext.UserId;
            txn.ReconciledDateUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(default(CancellationToken), true);
        }

        public async Task<PagedResult<TransactionDto>> SearchAsync(TransactionSearchDto filter)
        {
            var query = TransactionSearch(filter);
            
            var txns = await query.OrderByDescending(tr => tr.Date)
                .ThenByDescending(tr => tr.CreatedDateUtc)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Description = t.Description,
                    Date = t.Date,
                    Category = t.Category,
                    TypeId = t.TypeId,
                    Type = t.Type.Name,
                    TypeCode = t.Type.Code,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name,
                    Reconciled = t.Reconciled,
                    ReconciledById = t.ReconciledById,
                    ReconciledBy = $"{t.ReconciledBy.FirstName} {t.ReconciledBy.LastName}"
                }).GetPagedListAsync(filter);

            return txns;
        }

        public async Task<TransactionSummaryDto> GetTransactionSummary(TransactionSearchDto filter)
        {
            var query = TransactionSearch(filter);

            var summary = (await query.Include(t => t.Type).ToListAsync())
                .GroupBy(t => true)
                .Select(g => new TransactionSummaryDto
                {
                    Expenses = g.Where(t => t.Category == TransactionCategory.Expense).Sum(t => t.Amount),
                    Income = g.Where(t => t.Category == TransactionCategory.Income).Sum(t => t.Amount),
                    ShareDividend = g.Where(t => t.Category == TransactionCategory.Expense && t.Type.Code == "SHARE_DIVIDEND").Sum(t => t.Amount)
                }).FirstOrDefault() ?? new TransactionSummaryDto();

            summary.Profit = summary.Income + summary.Expenses;
            summary.Expenses -= summary.ShareDividend;

            return summary;
        }

        public async Task UnReconcileAsync(string id)
        {
            var txn = await _dbContext.Transactions.FindAsync(id);

            txn.Reconciled = false;
            txn.ReconciledById = null;
            txn.ReconciledDateUtc = null;

            await _dbContext.SaveChangesAsync(default(CancellationToken), true);
        }

        public async Task UpdateAsync(string id, TransactionDto dto)
        {
            var txn = await _dbContext.Transactions.FindAsync(id);

            if (txn.Reconciled)
                throw new SapException("ERR_CANT_UPDATE_RECONCILED_TXN");

            txn.Amount = dto.Category == TransactionCategory.Expense ? Math.Abs(dto.Amount) * -1 : Math.Abs(dto.Amount);
            txn.Description = dto.Description;
            txn.Category = dto.Category;
            txn.TypeId = dto.TypeId;
            txn.Date = dto.Date;
            txn.ProjectId = dto.ProjectId;

            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<Transaction> TransactionSearch(TransactionSearchDto filter)
        {
            var searchTerm = filter.SearchTerm?.ToLower() ?? "";
            var projects = string.IsNullOrEmpty(filter.Projects) ? new string[] { } : filter.Projects.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());

            var hasAllProjectAccess = _requestContext.HasPermission(CustomClaims.ProjectsFullAccess);

            var txns = _dbContext.Transactions
                .Where(t => (projects.Contains("*") && _requestContext.HasPermission(CustomClaims.ProjectsFullAccess) || projects.Contains(t.ProjectId))
                    && (filter.FromDate == null || t.Date >= filter.FromDate)
                    && (filter.ToDate == null || t.Date <= filter.ToDate)
                    && (filter.Category == null || t.Category == filter.Category)
                    && (filter.CategotyTypes == null || !filter.CategotyTypes.Any() || filter.CategotyTypes.Contains(t.TypeId))
                    && (filter.Reconsiled == null || filter.Reconsiled == t.Reconciled)
                    && (searchTerm == ""
                        || t.Description.ToLower().Contains(searchTerm)
                        || t.Project.ProjectTags.Any(pt => pt.Tag.Name.ToLower().Contains(searchTerm))));

            return txns;
        }
    }
}
