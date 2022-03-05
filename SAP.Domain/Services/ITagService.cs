using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Domain.Services
{
    public interface ITagService
    {
        Task<List<ListItemDto>> SearchAsync(string searchTerm);
        Task<string> CreateAsync(string names);
    }
}
