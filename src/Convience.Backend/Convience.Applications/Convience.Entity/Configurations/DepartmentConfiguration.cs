using Convience.Entity.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convience.Entity.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(d => d.Name).HasMaxLength(15);
            builder.Property(d => d.Email).HasMaxLength(50);
            builder.Property(d => d.Telephone).HasMaxLength(20);
        }
    }
}
