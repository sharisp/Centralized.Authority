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
