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
  
    public class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {

            builder.ToTable("T_Menus");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasQueryFilter(t => t.IsDel == false);
        }
    }
}
