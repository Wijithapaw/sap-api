using Microsoft.EntityFrameworkCore;
using SAP.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAP.Domain
{
    public interface IDbContext: IDisposable
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<LookupHeader> LookupHeaders { get; set; }
        DbSet<Lookup> Lookups { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
