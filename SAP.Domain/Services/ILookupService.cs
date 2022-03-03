﻿using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Domain.Services
{
    public interface ILookupService
    {
        Task<List<KeyValuePair<string, string>>> GetAllLookupHeadersAsync();
        Task<List<LookupDto>> GetByHeaderIdAsync(string headerId);
        Task<List<KeyValuePair<string, string>>> GetActiveLookupsAsListItemseAsync(string headerCode);
        Task<LookupDto> GetAsync(string id);
        Task<string> CreateAsync(LookupDto lookup);
        Task UpdateAsync(string id, LookupDto lookup);
        Task DeleteAsync(string id);
    }
}
