using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

            //Indexes
            builder.Entity<LookupHeader>().HasIndex(e => e.Code).IsUnique();
            builder.Entity<Lookup>().HasIndex(e => new { e.HeaderId, e.Code }).IsUnique();
            builder.Entity<Tag>().HasIndex(e => e.Name).IsUnique();
            builder.Entity<ProjectTag>().HasIndex(e => new { e.ProjectId, e.TagId }).IsUnique();

            //Set the kind of Datetime object's to UTC for UTC dates
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if ((property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        && property.Name.ToLower().EndsWith("utc"))
                        property.SetValueConverter(dateTimeConverter);
                }
            }
        }

        //Entities
        public DbSet<Project> Projects { get; set; }
        public DbSet<LookupHeader> LookupHeaders { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProjectTag> ProjectTags { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }

        public override int SaveChanges()
        {
            if (_requestContext != null)
            {
                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Added))
                {
                    entry.Entity.CreatedById = entry.Entity.LastUpdatedById = _requestContext.UserId;
                    entry.Entity.CreatedDateUtc = entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }

                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Entity.LastUpdatedById = _requestContext.UserId;
                    entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken), bool preventAuditData = false)
        {
            if (_requestContext != null && !preventAuditData)
            {
                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Added))
                {
                    entry.Entity.CreatedById = entry.Entity.LastUpdatedById = _requestContext.UserId;
                    entry.Entity.CreatedDateUtc = entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }

                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Entity.LastUpdatedById = _requestContext.UserId;
                    entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
