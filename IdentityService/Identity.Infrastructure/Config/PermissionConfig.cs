namespace Identity.Infrastructure.Config
{

    public class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {

            builder.ToTable("T_Permissions");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasQueryFilter(t => t.IsDel == false);
        }
    }
}
