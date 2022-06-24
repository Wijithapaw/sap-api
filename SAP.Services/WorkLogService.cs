using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Dtos;
using SAP.Domain.Entities;
using SAP.Domain.Enums;
using SAP.Domain.Exceptions;
using SAP.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Services
{
    public class WorkLogService : IWorkLogService
    {
        private readonly IDbContext _dbContext;
        private readonly ITransactionService _transactionService;
        private readonly ILookupService _lookupService;

        public WorkLogService(IDbContext dbContext, 
            ITransactionService transactionService,
            ILookupService lookupService)
        {
            _dbContext = dbContext;
            _transactionService = transactionService;
            _lookupService = lookupService;
        }

        public async Task<string> CreateAsync(WorkLogDto data)
        {
            var worklog = new WorkLog
            {
                ProjectId = data.ProjectId,
                Date = data.Date,
                LabourName = data.LabourName,
                JobDescription = data.JobDescription,
                Wage = data.Wage,
            };

            _dbContext.WorkLogs.Add(worklog);

            await _dbContext.SaveChangesAsync();

            if(data.CreateWageTxn && data.Wage != null)
            {
                var txn = new TransactionDto
                {
                    Amount = data.Wage.Value,
                    Date = data.Date,
                    Description = data.JobDescription,
                    Category = TransactionCategory.Expense,
                    ProjectId = data.ProjectId,
                    TypeId = await _lookupService.GetLookupIdAsync("EXPENSE_TYPES", "LABOUR"),
                };

                var txnId = await _transactionService.CreateAsync(txn);

                worklog.TransactionId = txnId;

                await _dbContext.SaveChangesAsync();
            }

            return worklog.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var log = await _dbContext.WorkLogs.FindAsync(id);

            if(log.TransactionId != null)
                await _transactionService.DeleteAsync(log.TransactionId);

            if (log != null)
            {
                _dbContext.WorkLogs.Remove(log);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<WorkLogDto> GetAsync(string id)
        {
            var log = await _dbContext.WorkLogs
                .Where(l => l.Id == id)
                .Select(l => new WorkLogDto
                {
                    Id = l.Id,
                    Date = l.Date,
                    JobDescription = l.JobDescription,
                    LabourName = l.LabourName,
                    ProjectId = l.ProjectId,
                    ProjectName = l.Project.Name,
                    Wage = l.Wage
                }).FirstOrDefaultAsync();

            return log;
        }

        public async Task<PagedResult<WorkLogDto>> SearchAsync(WorkLogSearchDto filter)
        {
            var searchTerm = filter.SearchTerm?.ToLower() ?? "";
            var projects = string.IsNullOrEmpty(filter.Projects) ? new string[] { } : filter.Projects.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());

            var logs = await _dbContext.WorkLogs
                .Where(l => (filter.Projects == "*" || projects.Contains(l.ProjectId))
                    && (filter.From == null || l.Date >= filter.From)
                    && (filter.To == null || l.Date <= filter.To)
                    && (searchTerm == "" || l.LabourName.ToLower().Contains(searchTerm) || l.JobDescription.ToLower().Contains(searchTerm)))
                .OrderByDescending(l => l.Date)
                .ThenByDescending(l => l.CreatedDateUtc)
                .Select(l => new WorkLogDto
                {
                    Id = l.Id,
                    Date = l.Date,
                    JobDescription = l.JobDescription,
                    LabourName = l.LabourName,
                    ProjectId = l.ProjectId,
                    ProjectName = l.Project.Name,
                    Wage = l.Wage
                }).GetPagedListAsync(filter);

            return logs;
        }

        public async Task UpdateAsync(string id, WorkLogDto data)
        {
            var log = await _dbContext.WorkLogs.FindAsync(id);

            if (log != null)
            {
                if (log.TransactionId != null)
                {
                    var txn = new TransactionDto
                    {
                        Amount = data.Wage.Value,
                        Date = data.Date,
                        Description = data.JobDescription,
                        Category = TransactionCategory.Expense,
                        ProjectId = data.ProjectId,
                        TypeId = await _lookupService.GetLookupIdAsync("EXPENSE_TYPES", "LABOUR"),
                    };

                    await _transactionService.UpdateAsync(log.TransactionId, txn);
                }                    

                log.ProjectId = data.ProjectId;
                log.Date = data.Date;
                log.LabourName = data.LabourName;
                log.JobDescription = data.JobDescription;
                log.Wage = data.Wage;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
