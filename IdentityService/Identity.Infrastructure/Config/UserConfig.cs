namespace Identity.Infrastructure.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.ToTable("T_Users");
            builder.OwnsOne(e => e.Phone, t =>
            {
                t.Property(p => p.Number).HasMaxLength(100).IsRequired();
                t.Property(p => p.CountryCode).HasMaxLength(20).IsRequired();
            });
            //  builder.("T_Users");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.UserName).HasMaxLength(50).IsUnicode(false).IsRequired();
            builder.Property(e => e.NickName).HasMaxLength(50);
            builder.Property(e => e.RealName).HasMaxLength(50);
            builder.Property(e => e.Email).HasMaxLength(50).IsRequired().IsRequired();
            builder.Property(e => e.RefreshToken).HasMaxLength(36).IsUnicode();
            builder.Property(e => e.PasswordHash).HasMaxLength(100).IsRequired();

            builder.HasMany(e => e.Roles).WithMany(t => t.Users).UsingEntity(j => j.ToTable("T_UserRole"));

            builder.HasQueryFilter(t => t.IsDel == false);
        }
    }
}