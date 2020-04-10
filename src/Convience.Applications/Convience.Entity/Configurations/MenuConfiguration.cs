using Convience.Entity.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convience.Entity.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.Property(m => m.Name).HasMaxLength(15);
            builder.Property(m => m.Identification).HasMaxLength(50);
            builder.Property(m => m.Permission).HasMaxLength(200);
            builder.Property(m => m.Route).HasMaxLength(50);
        }
    }
}
