using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Config
{
    public class SystemConfConfig : IEntityTypeConfiguration<SystemConfig>
    {
        public void Configure(EntityTypeBuilder<SystemConfig> builder)
        {
            builder.ToTable("T_Systems_Config");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.SystemName)
       .HasMaxLength(100);

            builder.Property(e => e.Description)
                   .HasMaxLength(500);
            builder.Property(e => e.ConfigKey)
                         .HasMaxLength(100);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.HasQueryFilter(t => t.IsActive == true);
            builder.HasQueryFilter(t => t.IsDel == false);

        }
    }
}