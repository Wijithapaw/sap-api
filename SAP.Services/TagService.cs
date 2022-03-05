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
    public class TagService : ITagService
    {
        private readonly IDbContext _dbContext;

        public TagService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateAsync(string name)
        {
            name = name.ToLower().Trim();

            var tag = new Tag { Name = name };
            _dbContext.Tags.Add(tag);

            await _dbContext.SaveChangesAsync();

            return tag.Id;
        }

        public async Task<List<ListItemDto>> SearchAsync(string searchTerm)
        {
            searchTerm = searchTerm?.ToLower() ?? string.Empty;
            var tags = await _dbContext.Tags
                .Where(t => t.Name.ToLower().Contains(searchTerm.ToLower()))
                .Select(t => new ListItemDto(t.Id, t.Name))
                .ToListAsync();

            return tags;
        }
    }
}
