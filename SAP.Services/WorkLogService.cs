using Microsoft.EntityFrameworkCore;
using SAP.Domain;
using SAP.Domain.Dtos;
using SAP.Domain.Entities;
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

        public WorkLogService(IDbContext dbContext)
        {
            _dbContext = dbContext;
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

            return worklog.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var log = await _dbContext.WorkLogs.FindAsync(id);

            if (log != null)
            {
                _dbContext.WorkLogs.Remove(log);
            }

            await _dbContext.SaveChangesAsync();
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
