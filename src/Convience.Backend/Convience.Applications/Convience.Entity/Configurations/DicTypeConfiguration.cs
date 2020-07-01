using Convience.Entity.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convience.Entity.Configurations
{
    public class DicTypeConfiguration : IEntityTypeConfiguration<DicType>
    {
        public void Configure(EntityTypeBuilder<DicType> builder)
        {
            builder.Property(a => a.Name).IsRequired().HasMaxLength(15);
            builder.Property(a => a.Code).IsRequired().HasMaxLength(15);
            builder.HasIndex(a => a.Code).IsUnique();
        }
    }
}