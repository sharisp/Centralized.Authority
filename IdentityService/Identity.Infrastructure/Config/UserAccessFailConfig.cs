namespace Identity.Infrastructure.Config
{

    public class UserAccessFailConfig : IEntityTypeConfiguration<UserAccessFail>
    {
        public void Configure(EntityTypeBuilder<UserAccessFail> builder)
        {

            builder.ToTable("T_UserAccessFails");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
        }
    }
}
