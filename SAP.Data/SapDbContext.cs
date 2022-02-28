using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SAP.Domain;
using SAP.Domain.Dtos;
using SAP.Domain.Entities;
using SAP.Domain.Entities.Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SAP.Data
{
    public class SapDbContext : IdentityDbContext<User, Role, string>, IDbContext
    {
        private readonly IRequestContext _requestContext;

        public SapDbContext(DbContextOptions<SapDbContext> options, IRequestContext requestContext)
           : base(options)
        {
            _requestContext = requestContext;
        }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            var keysProperties = builder.Model.GetEntityTypes()
               .Select(e => e.FindPrimaryKey())
               .Where(key => key != null)
               .SelectMany(x => x.Properties)
               .ToList();

            foreach (var property in keysProperties)
            {
                property.ValueGenerated = ValueGenerated.OnAdd;
            }
        }

        public DbSet<Project> Projects { get; set; }

        public override int SaveChanges()
        {
            if (_requestContext != null)
            {
                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Added))
                {
                    entry.Entity.CreatedBy = entry.Entity.LastUpdatedBy = _requestContext.UserId;
                    entry.Entity.CreatedDateUtc = entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }

                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Entity.LastUpdatedBy = _requestContext.UserId;
                    entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_requestContext != null)
            {
                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Added))
                {
                    entry.Entity.CreatedBy = entry.Entity.LastUpdatedBy = _requestContext.UserId;
                    entry.Entity.CreatedDateUtc = entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }

                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Entity.LastUpdatedBy = _requestContext.UserId;
                    entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }            
            }           
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
