using Identity.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Config
{

    public class LoginHistoryConfig:IEntityTypeConfiguration<LoginHistory>
    {
        public void Configure(EntityTypeBuilder<LoginHistory> builder)
        {

            builder.ToTable("T_LoginHistory");
            builder.OwnsOne(e => e.Phone, t =>
            {
                t.Property(p => p.Number).HasMaxLength(100).IsRequired();
                t.Property(p => p.CountryCode).HasMaxLength(20).IsRequired();
            });
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
        }
    }
}
