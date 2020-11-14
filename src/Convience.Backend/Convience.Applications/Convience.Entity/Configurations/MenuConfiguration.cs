using Convience.Entity.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convience.Entity.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            // int主键时在表中有初始化数据的情况下，需要指定开始的序列值。不进行设置的话，数据主键
            // 会从1开始从而导致主键冲突。
            builder.Property(m => m.Id).HasIdentityOptions(startValue: 85);
            builder.Property(m => m.Name).HasMaxLength(15);
            builder.Property(m => m.Identification).HasMaxLength(50);
            builder.Property(m => m.Permission).HasMaxLength(2000);
            builder.Property(m => m.Route).HasMaxLength(50);
        }
    }
}
