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

        [HttpGet("AllHeaders")]
        [ClaimAuthorize(CustomClaims.LookupsFullAccess)]
        public async Task<List<ListItemDto>> GetAllHeaders()
        {
            return await _lookupService.GetAllLookupHeadersAsync();
        }

        [HttpGet("ByHeader/{headerId}")]
        [ClaimAuthorize(CustomClaims.LookupsFullAccess)]
        public async Task<List<LookupDto>> GetAllByHeaderId(string headerId)
        {
            return await _lookupService.GetByHeaderIdAsync(headerId);
        }

        [HttpGet("Active/ListItems/{headercode}")]
        public async Task<List<ListItemDto>> GetListItemsByHeader(string headerCode)
        {
            return await _lookupService.GetActiveLookupsAsListItemseAsync(headerCode);
        }

        [HttpGet("{id}")]
        [ClaimAuthorize(CustomClaims.LookupsFullAccess)]
        public async Task<LookupDto> Get(string id)
        {
            return await _lookupService.GetAsync(id);
        }

        [HttpGet("Header/ByCode/{code}")]
        [ClaimAuthorize(CustomClaims.LookupsFullAccess)]
        public async Task<LookupHeaderDto> GetHeader(string code)
        {
            return await _lookupService.GetHeaderByCodeAsync(code);
        }

        [HttpPost]
        [ClaimAuthorize(CustomClaims.LookupsFullAccess)]
        public async Task<string> Create([FromBody] LookupDto lookup)
        {
            return await _lookupService.CreateAsync(lookup);
        }

        [HttpPut("{id}")]
        [ClaimAuthorize(CustomClaims.LookupsFullAccess)]
        public async Task Update(string id, [FromBody] LookupDto value)
        {
            await _lookupService.UpdateAsync(id, value);
        }

        [HttpDelete("{id}")]
        [ClaimAuthorize(CustomClaims.LookupsFullAccess)]
        public async void Delete(string id)
        {
            await _lookupService.DeleteAsync(id);
        }
    }
}
