using Microsoft.AspNetCore.Mvc;
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
        public async Task<List<ProjectDto>> Get(string searchTerm, bool activeOnly)
        {
            return await _projectService.SearchAsync(searchTerm, activeOnly);
        }

        [HttpGet("listitems")]
        public async Task<List<ListItemDto>> GetListItems(string searchTerm)
        {
            return await _projectService.GetProjectsListItemsAsync(searchTerm);
        }

        [HttpGet("{id}")]
        public async Task<ProjectDto> Get(string id)
        {
            return await _projectService.GetAsync(id);
        }

        [HttpPost]
        public async Task<string> PostAsync([FromBody] ProjectDto value)
        {
            return await _projectService.CreateAsync(value);
        }

        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] ProjectDto value)
        {
            await _projectService.UpdateAsync(id, value);
        }

        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _projectService.DeleteAsync(id);
        }
    }
}
