namespace Identity.Infrastructure.Config
{
    internal class SysConfig : IEntityTypeConfiguration<Sys>
    {
        public void Configure(EntityTypeBuilder<Sys> builder)
        {
            builder.ToTable("T_Systems");
       
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.HasQueryFilter(t => t.IsDel == false);
        }
    }
}
