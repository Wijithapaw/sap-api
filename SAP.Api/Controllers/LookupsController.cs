using Microsoft.AspNetCore.Mvc;
using SAP.Domain.Dtos;
using SAP.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookupsController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("allheaders")]
        public async Task<List<KeyValuePair<string, string>>> GetAllHeaders()
        {
            return await _lookupService.GetAllLookupHeadersAsync();
        }

        [HttpGet("byheader/{headerId}")]
        public async Task<List<LookupDto>> GetAllByHeaderId(string headerId)
        {
            return await _lookupService.GetByHeaderIdAsync(headerId);
        }

        [HttpGet("/active/listitems/{headercode}")]
        public async Task<List<KeyValuePair<string, string>>> GetListItemsByHeader(string headerCode)
        {
            return await _lookupService.GetActiveLookupsAsListItemseAsync(headerCode);
        }

        [HttpGet("{id}")]
        public async Task<LookupDto> Get(string id)
        {
            return await _lookupService.GetAsync(id);
        }

        [HttpPost]
        public async Task<string> Create([FromBody] LookupDto lookup)
        {
            return await _lookupService.CreateAsync(lookup);
        }

        [HttpPut("{id}")]
        public async Task Update(string id, [FromBody] LookupDto value)
        {
            await _lookupService.UpdateAsync(id, value);
        }

        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _lookupService.DeleteAsync(id);
        }
    }
}
