using SAP.Domain.Dtos;
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
        Task<PagedResult<TransactionDto>> SearchAsync(TransactionSearchDto filter);
        Task<TransactionDto> GetAsync(string id);
        Task ReconcileAsync(string id);
        Task UnReconcileAsync(string id);
        Task<TransactionSummaryDto> GetTransactionSummary(TransactionSearchDto filter);
    }
}
