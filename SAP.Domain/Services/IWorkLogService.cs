using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Domain.Services
{
    public interface IWorkLogService
    {
        Task<string> CreateAsync(WorkLogDto data);
        Task UpdateAsync(string id, WorkLogDto data);
        Task<WorkLogDto> GetAsync(string id);
        Task DeleteAsync(string id);
        Task<PagedResult<WorkLogDto>> SearchAsync(WorkLogSearchDto filter);
        Task<List<string>> GetLabourNamesSuggestionsAsync(string prefix);
    }
}
