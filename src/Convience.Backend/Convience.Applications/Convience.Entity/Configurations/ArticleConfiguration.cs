using Convience.Entity.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace Convience.Entity.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(a => a.Title).IsRequired();
            builder.Property(a => a.Title).HasMaxLength(50);
            builder.Property(a => a.SubTitle).HasMaxLength(200);
            builder.Property(a => a.Source).HasMaxLength(200);
            builder.Property(a => a.Tags).HasMaxLength(200);

            builder.Property(a => a.CreateTime).HasDefaultValue(DateTime.Now);
            builder.Property(a => a.UpdateTime).HasDefaultValue(DateTime.Now);
        }
    }
}
