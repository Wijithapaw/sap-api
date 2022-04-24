﻿using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Domain.Services
{
    public interface ITransactionService
    {
        Task<string> CreateAsync(TransactionDto transaction);
        Task UpdateAsync(string id, TransactionDto transaction);
        Task DeleteAsync(string id);
        Task<List<TransactionDto>> SearchAsync(TransactionSearchDto filter);
        Task<PagedResult<TransactionDto>> SearchAsync2(TransactionSearchDto filter);
        Task<TransactionDto> GetAsync(string id);
        Task ReconcileAsync(string id);
        Task UnReconcileAsync(string id);
        Task<TransactionSummaryDto> GetTransactionSummary(TransactionSearchDto filter);
    }
}
