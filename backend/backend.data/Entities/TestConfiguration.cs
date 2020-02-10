using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace backend.data.Entities
{
    public class TestConfiguration : IEntityTypeConfiguration<TestEntity>
    {
        public void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Desc).HasMaxLength(20).IsRequired();
        }
    }
}
