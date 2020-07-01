using Convience.Entity.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convience.Entity.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.Property(m => m.Name).HasMaxLength(15);
            builder.Property(m => m.UrlPrefix).HasMaxLength(20);
            builder.Property(m => m.ConnectionString).HasMaxLength(150);
        }
    }
}
