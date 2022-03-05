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
    public class LookupService : ILookupService
    {
        private readonly IDbContext _dbContext;

        public LookupService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateAsync(LookupDto dto)
        {
            var lookup = new Lookup
            {
                Code = dto.Code,
                HeaderId = dto.HeaderId,
                Inactive = dto.Inactive,
                Name = dto.Name,
            };

            _dbContext.Lookups.Add(lookup);

            await _dbContext.SaveChangesAsync();

            return lookup.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var lookup = await _dbContext.Lookups.FindAsync(id);

            _dbContext.Lookups.Remove(lookup);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ListItemDto>> GetAllLookupHeadersAsync()
        {
            var headers = await _dbContext.LookupHeaders
                .Select(h => new ListItemDto(h.Id, h.Name))
                .ToListAsync();

            return headers;
        }

        public async Task<LookupDto> GetAsync(string id)
        {
            var lookup = await _dbContext.Lookups
                .Where(l => l.Id == id)
                .Select(l => new LookupDto
                {
                    Id = l.Id,
                    Code = l.Code,
                    HeaderId = l.HeaderId,
                    Name = l.Name,
                    Inactive = l.Inactive
                }).FirstOrDefaultAsync();

            return lookup;
        }

        public async Task<List<LookupDto>> GetByHeaderIdAsync(string headerId)
        {
            var lookups = await _dbContext.Lookups
                .Where(l => l.HeaderId == headerId)
                .Select(l => new LookupDto
                {
                    Id = l.Id,
                    Code = l.Code,
                    HeaderId = l.HeaderId,
                    Name = l.Name,
                    Inactive = l.Inactive
                }).ToListAsync();

            return lookups;
        }

        public async Task<List<ListItemDto>> GetActiveLookupsAsListItemseAsync(string headerCode)
        {
            var lookups = await _dbContext.Lookups
               .Where(l => l.Header.Code == headerCode && !l.Inactive)
               .Select(h => new ListItemDto(h.Id, h.Name))
               .ToListAsync();

            return lookups;
        }

        public async Task UpdateAsync(string id, LookupDto dto)
        {
            var lookup = await _dbContext.Lookups.FindAsync(id);

            lookup.Code = dto.Code;
            lookup.Name = dto.Name;
            lookup.Inactive = dto.Inactive;

            await _dbContext.SaveChangesAsync();                 
        }
    }
}
