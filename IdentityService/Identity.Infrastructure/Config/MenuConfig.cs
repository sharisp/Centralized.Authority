namespace Identity.Infrastructure.Config
{

    public class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {

            builder.ToTable("T_Menus");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.HasMany(t => t.Permissions)
                .WithMany(e => e.Menus)
                .UsingEntity(j => j.ToTable("T_MenuPermissions"));
            builder.HasQueryFilter(t => t.IsDel == false);
        }
    }
}
