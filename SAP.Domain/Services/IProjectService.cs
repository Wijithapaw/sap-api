using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Domain.Services
{
    public interface IProjectService
    {
        Task<string> CreateAsync(ProjectDto project);
        Task DeleteAsync(string id);
        Task<List<ProjectDto>> SearchAsync(string searchTerm = null, bool activeOnly = false);
        Task<List<ListItemDto>> GetProjectsListItemsAsync(string searchTerm = null);
        Task<ProjectDto> GetAsync(string id);
        Task UpdateAsync(string id, ProjectDto project);
    }
}
