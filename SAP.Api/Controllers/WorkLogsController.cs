using Microsoft.AspNetCore.Mvc;
using SAP.Domain.Dtos;
using SAP.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLogsController : ControllerBase
    {
        private readonly IWorkLogService _workLogService;

        public WorkLogsController(IWorkLogService workLogService)
        {
            _workLogService = workLogService;
        }

        [HttpGet]
        public async Task<PagedResult<WorkLogDto>> Search([FromQuery] WorkLogSearchDto filter)
        {
            return await _workLogService.SearchAsync(filter);
        }

        [HttpGet("{id}")]
        public async Task<WorkLogDto> Get(string id)
        {
            return await _workLogService.GetAsync(id);
        }

        [HttpGet("Labours/{prefix}")]
        public async Task<List<string>> GetLabourNameSuggestions(string prefix)
        {
            return await _workLogService.GetLabourNamesSuggestionsAsync(prefix);
        }

        [HttpPut("{id}")]
        public async Task Update(string id, WorkLogDto value)
        {
            await _workLogService.UpdateAsync(id, value);
        }

        [HttpPost]
        public async Task<string> Create(WorkLogDto value)
        {
            return await _workLogService.CreateAsync(value);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _workLogService.DeleteAsync(id);
        }
    }
}
