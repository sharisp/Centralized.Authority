namespace Identity.Infrastructure.Repository
{
    public class BaseDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<WhiteUrl> WhiteUrls { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<Sys> Sys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);

            // Add any custom configurations here
            // For example, you can configure entity properties, relationships, etc.
        }

       
       
    }
}
