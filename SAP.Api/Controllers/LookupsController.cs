using Microsoft.AspNetCore.Authorization;
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
    public class LookupsController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookupsController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("allheaders")]
        [ClaimAuthorize(CustomClaims.LookupsManage)]
        public async Task<List<KeyValuePair<string, string>>> GetAllHeaders()
        {
            return await _lookupService.GetAllLookupHeadersAsync();
        }

        [HttpGet("byheader/{headerId}")]
        [ClaimAuthorize(CustomClaims.LookupsManage)]
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
        [ClaimAuthorize(CustomClaims.LookupsManage)]
        public async Task<LookupDto> Get(string id)
        {
            return await _lookupService.GetAsync(id);
        }

        [HttpPost]
        [ClaimAuthorize(CustomClaims.LookupsManage)]
        public async Task<string> Create([FromBody] LookupDto lookup)
        {
            return await _lookupService.CreateAsync(lookup);
        }

        [HttpPut("{id}")]
        [ClaimAuthorize(CustomClaims.LookupsManage)]
        public async Task Update(string id, [FromBody] LookupDto value)
        {
            await _lookupService.UpdateAsync(id, value);
        }

        [HttpDelete("{id}")]
        [ClaimAuthorize(CustomClaims.LookupsManage)]
        public async void Delete(string id)
        {
            await _lookupService.DeleteAsync(id);
        }
    }
}
