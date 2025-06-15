using Identity.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
