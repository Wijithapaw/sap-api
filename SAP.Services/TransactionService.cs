﻿using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Constants;
using SAP.Domain.Dtos;
using SAP.Domain.Entities;
using SAP.Domain.Enums;
using SAP.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<string> CreateAsync(TransactionDto dto, bool autoReconcile = false)
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

            if(autoReconcile && _requestContext.HasPermission(CustomClaims.TransactionReconcile))
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

            _dbContext.Transactions.Remove(txn);

            await _dbContext.SaveChangesAsync();
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
                    ReconciledBy = $"{t.ReconciledBy.FirstName} {t.ReconciledBy.LastName}" 
                }).FirstOrDefaultAsync();

            return txn;
        }

        public async Task ReconcileAsync(string id)
        {
            var txn = await _dbContext.Transactions.FindAsync(id);

            txn.Reconciled = true;
            txn.ReconciledById = _requestContext.UserId;            

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TransactionDto>> SearchAsync(TransactionSearchDto filter)
        {
            var searchTerm = filter.SearchTerm?.ToLower() ?? "";
            var projects = string.IsNullOrEmpty(filter.Projects) ? new string[] { } : filter.Projects.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());

            var hasAllProjectAccess = _requestContext.HasPermission(CustomClaims.ProjectsFullAccess);

            var txns = await _dbContext.Transactions
                .Where(t => (projects.Contains("*") && _requestContext.HasPermission(CustomClaims.ProjectsFullAccess) || projects.Contains(t.ProjectId))
                    && (filter.FromDate == null || t.Date >= filter.FromDate)
                    && (filter.ToDate == null || t.Date <= filter.ToDate)
                    && (filter.Category == null || t.Category == filter.Category)
                    && (filter.CategotyTypes == null || !filter.CategotyTypes.Any() || filter.CategotyTypes.Contains(t.TypeId))
                    && (filter.Reconsiled == null || filter.Reconsiled == t.Reconciled)
                    && (searchTerm == ""
                        || t.Description.ToLower().Contains(searchTerm)
                        || t.Project.ProjectTags.Any(pt => pt.Tag.Name.ToLower().Contains(searchTerm))))
                .OrderByDescending(tr => tr.Date)
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
                })
                .ToListAsync();

            return txns;
        }

        public async Task UnReconcileAsync(string id)
        {
            var txn = await _dbContext.Transactions.FindAsync(id);

            txn.Reconciled = false;
            txn.ReconciledById = null;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, TransactionDto dto)
        {
            var txn = await _dbContext.Transactions.FindAsync(id);

            txn.Amount = dto.Category == TransactionCategory.Expense ? Math.Abs(dto.Amount) * -1 : Math.Abs(dto.Amount);
            txn.Description = dto.Description;
            txn.Category = dto.Category;
            txn.TypeId = dto.TypeId;
            txn.Date = dto.Date;
            txn.ProjectId = dto.ProjectId;
            txn.Reconciled = false;
            txn.ReconciledById = null;

            await _dbContext.SaveChangesAsync();
        }
    }
}
