namespace Identity.Infrastructure.Config
{

    public class WhiteUrlConfig : IEntityTypeConfiguration<WhiteUrl>
    {
        public void Configure(EntityTypeBuilder<WhiteUrl> builder)
        {

            builder.ToTable("T_WhiteUrls");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
        }
    }
}
