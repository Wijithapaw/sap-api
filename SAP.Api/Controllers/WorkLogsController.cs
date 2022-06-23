using Microsoft.AspNetCore.Mvc;
using SAP.Domain.Dtos;
using SAP.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
