using Microsoft.EntityFrameworkCore;
using SAP.Domain;
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
    public class ProjectService : IProjectService
    {
        private readonly IDbContext _dbContext;

        public ProjectService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateAsync(ProjectDto dto)
        {
            var newProject = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                State = dto.State,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ProjectManagerId = dto.ProjectManagerId
            };

            _dbContext.Projects.Add(newProject);

            await _dbContext.SaveChangesAsync();

            return newProject.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var project = await _dbContext.Projects.FindAsync(id);

            _dbContext.Projects.Remove(project);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProjectDto> GetAsync(string id)
        {
            var project = await _dbContext.Projects
                    .Where(p => p.Id == id)
                    .Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        State = p.State,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        ProjectManagerId = p.ProjectManagerId,
                        ProjectManager = $"{p.ProjectManager.FirstName} {p.ProjectManager.LastName}"
                    }).FirstOrDefaultAsync();

            return project;
        }

        public async Task<List<ProjectDto>> SearchAsync(string searchTerm = null, bool activeOnly = false)
        {
            var projects = await SearchQuery(searchTerm, activeOnly)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    State = p.State,
                    ProjectManager = $"{p.ProjectManager.FirstName} {p.ProjectManager.LastName}"
                }).ToListAsync();

            return projects;
        }

        public async Task<List<ListItemDto>> GetProjectsListItemsAsync(string searchTerm = null)
        {
            var projects = await SearchQuery(searchTerm, false)
                .Select(p => new ListItemDto
                {
                    Key = p.Id,
                    Value = p.Title
                   
                }).ToListAsync();

            return projects;
        }

        public async Task UpdateAsync(string id, ProjectDto dto)
        {
            var project = await _dbContext.Projects.FindAsync(id);

            if (project == null)
                throw new ApplicationException("ERR_RECORD_NOT_FOUND");

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;
            project.State = dto.State;
            project.ProjectManagerId = dto.ProjectManagerId;

            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<Project> SearchQuery(string searchTerm = null, bool activeOnly = false)
        {
            searchTerm = searchTerm?.Trim().ToLower();

            var activeStates = new ProjectState[] { ProjectState.Pending, ProjectState.Inprogress };

            var projects = _dbContext.Projects
                .Where(p => (!activeOnly || activeStates.Contains(p.State))
                    && (string.IsNullOrEmpty(searchTerm)
                        || p.Title.ToLower().Contains(searchTerm)
                        || p.Description.ToLower().Contains(searchTerm)));

            return projects;
        }
    }
}
