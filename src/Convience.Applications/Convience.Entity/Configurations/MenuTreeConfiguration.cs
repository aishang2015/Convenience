using Convience.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Entity.Configurations
{
    public class MenuTreeConfiguration : IEntityTypeConfiguration<MenuTree>
    {
        public void Configure(EntityTypeBuilder<MenuTree> builder)
        {
            builder.Property(m => m.Id).HasIdentityOptions(startValue: 109);
        }
    }
}
