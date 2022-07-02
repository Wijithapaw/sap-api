using Microsoft.AspNetCore.Mvc;
using SAP.Api.Authorization;
using SAP.Domain.Constants;
using SAP.Domain.Dtos;
using SAP.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [ClaimAuthorize(CustomClaims.ProjectsFullAccess)]
        public async Task<List<ProjectDto>> Get(string searchTerm, bool activeOnly)
        {
            return await _projectService.SearchAsync(searchTerm, activeOnly);
        }

        [HttpGet("ListItems")]
        public async Task<List<ListItemDto>> GetListItems(string searchTerm)
        {
            return await _projectService.GetProjectsListItemsAsync(searchTerm);
        }

        [HttpGet("{id}")]
        [ClaimAuthorize(CustomClaims.ProjectsFullAccess)]
        public async Task<ProjectDto> Get(string id)
        {
            return await _projectService.GetAsync(id);
        }

        [HttpPost]
        [ClaimAuthorize(CustomClaims.ProjectsFullAccess)]
        public async Task<string> PostAsync([FromBody] ProjectDto value)
        {
            return await _projectService.CreateAsync(value);
        }

        [HttpPut("{id}")]
        [ClaimAuthorize(CustomClaims.ProjectsFullAccess)]
        public async Task Put(string id, [FromBody] ProjectDto value)
        {
            await _projectService.UpdateAsync(id, value);
        }

        [HttpDelete("{id}")]
        [ClaimAuthorize(CustomClaims.ProjectsFullAccess)]
        public async Task Delete(string id)
        {
            await _projectService.DeleteAsync(id);
        }

        [HttpPost("Tag/{projectId}")]
        [ClaimAuthorize(CustomClaims.ProjectsFullAccess)]
        public async Task AddTag(string projectId, ListItemDto tag)
        {
            await _projectService.AddTagAsync(projectId, tag);
        }
    }
}
