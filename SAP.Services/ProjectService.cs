using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Dtos;
using SAP.Domain.Enums;
using SAP.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IDbContext _dbContext;

        public ProjectService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<string> CreateAsync(ProjectDto project)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProjectDto>> SearchAsync(string searchTerm = null, bool activeOnly = false)
        {
            searchTerm = searchTerm?.Trim().ToLower();

            var activeStates = new ProjectState[] { ProjectState.Pending, ProjectState.Inprogress };

            var projects = await _dbContext.Projects
                .Where(p => (!activeOnly || activeStates.Contains(p.State))
                    && (string.IsNullOrEmpty(searchTerm)
                        || p.Title.ToLower().Contains(searchTerm)
                        || p.Description.ToLower().Contains(searchTerm)))
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    State = p.State
                }).ToListAsync();

            return projects;
        }

        public Task UpdateAsync(string id, ProjectDto project)
        {
            throw new NotImplementedException();
        }
    }
}
