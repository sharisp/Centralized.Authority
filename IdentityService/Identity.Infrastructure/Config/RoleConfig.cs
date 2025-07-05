namespace Identity.Infrastructure.Config
{
    internal class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("T_Roles");
            //  builder.("T_Users");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.RoleName).HasMaxLength(50).IsUnicode(false).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.HasMany(t => t.Permissions).WithMany(e => e.Roles).UsingEntity(j => j.ToTable("T_RolePermissions"));

            builder.HasMany(t => t.Menus).WithMany(e => e.Roles).UsingEntity(j => j.ToTable("T_RoleMenus"));
            builder.HasQueryFilter(t => t.IsDel == false);
        }
    }
}
