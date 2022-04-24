using Microsoft.EntityFrameworkCore;
using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Services
{
    public static class EFExtentions
    {
        public static PagedResult<T> GetPagedList<T>(this IQueryable<T> query,
                                         PagedSearch filter) where T : class
        {
            var result = new PagedResult<T>();
            result.Total = query.Count();

            var skip = (filter.Page - 1) * filter.PageSize;
            result.Items = query.Skip(skip).Take(filter.PageSize).ToList();

            return result;
        }

        public static async Task<PagedResult<T>> GetPagedListAsync<T>(this IQueryable<T> query,
                                        PagedSearch filter) where T : class
        {
            var result = new PagedResult<T>();
            result.Total = await query.CountAsync();

            var skip = (filter.Page - 1) * filter.PageSize;
            result.Items = await query.Skip(skip).Take(filter.PageSize).ToListAsync();

            return result;
        }
    }
}
