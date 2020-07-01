using Convience.Entity.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convience.Entity.Configurations
{
    public class DicDataConfiguration : IEntityTypeConfiguration<DicData>
    {
        public void Configure(EntityTypeBuilder<DicData> builder)
        {
            builder.Property(a => a.Name).HasMaxLength(15);
        }
    }
}