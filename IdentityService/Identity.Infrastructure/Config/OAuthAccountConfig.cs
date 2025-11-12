using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Config
{
    public class OAuthAccountConfig : IEntityTypeConfiguration<OAuthAccount>
    {
        public void Configure(EntityTypeBuilder<OAuthAccount> builder)
        {

            builder.ToTable("T_OAuthAccounts");
          
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Description).HasMaxLength(500);

            builder.HasQueryFilter(t => t.IsDel == false);
        }
    }
}
