using Microsoft.EntityFrameworkCore;
using SAP.Domain.Entities;
using System;

namespace SAP.Domain
{
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
    }
}
