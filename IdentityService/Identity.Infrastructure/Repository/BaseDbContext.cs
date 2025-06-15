using MediatR;
using Identity.Domain;
using Identity.Domain.Entity;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repository
{
    public class BaseDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly ICurrentUser currentUser;
        private readonly IMediator mediator;

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<WhiteUrl> WhiteUrls { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }

        public BaseDbContext(DbContextOptions options, ICurrentUser currentUser, IMediator mediatR) : base(options)
        {
            currentUser = currentUser;
            this.mediator= mediatR ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);

            // Add any custom configurations here
            // For example, you can configure entity properties, relationships, etc.
        }

        public override int SaveChanges()
        {
            UpdateEntities(this);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateEntities(this);
            var res = await base.SaveChangesAsync(cancellationToken);
            if (res > 0)
            {
                await DispatchEvents(this);
            }
            return res;
        }

        public void UpdateEntities(Microsoft.EntityFrameworkCore.DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    var now = DateTimeOffset.UtcNow;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatorUserId = currentUser?.UserId;
                            entry.Entity.CreateDateTime = now;
                            entry.Entity.IsDel = false;
                            break;

                        case EntityState.Modified:
                            if (entry.HasChangedOwnedEntities() || entry.Properties.Any(p => p.IsModified))
                            {
                                entry.Entity.UpdaterUserId = currentUser?.UserId;
                                entry.Entity.UpdateDateTime = now;
                            }
                            break;

                        case EntityState.Deleted:
                            // 软删除：标记删除，不直接从数据库删除
                            entry.State = EntityState.Modified;
                            entry.Entity.IsDel = true;
                            entry.Entity.DeleterUserId = currentUser?.UserId;
                            entry.Entity.DeleteDateTime = now;
                            break;
                    }
                }
            }
        }

        public async Task DispatchEvents(Microsoft.EntityFrameworkCore.DbContext? context)
        {
            if (context == null) return;

            var entities = context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.GetDomainEvents().Any())
                .Select(e => e.Entity);

            var domainEvents = entities
                .SelectMany(e => e.GetDomainEvents())
                .ToList();

            entities.ToList().ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);

        }
    }
}
