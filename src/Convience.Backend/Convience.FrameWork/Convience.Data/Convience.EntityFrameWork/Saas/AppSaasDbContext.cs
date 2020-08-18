using Convience.EntityFrameWork.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Convience.EntityFrameWork.Saas
{
    public class AppSaasDbContext : DbContext
    {
        public string Schema { get; private set; } = "temp";

        public AppSaasDbContext(DbContextOptions<AppSaasDbContext> options, ISchemaService schemaService) : base(options)
        {
            if (!string.IsNullOrEmpty(schemaService.Schema))
            {
                Schema = schemaService.Schema;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ConfigurationEntity(typeof(AppSaasDbContext));
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // dbcontext对象在程序启动时会被实例化并缓存，刷新dbcontext需要根据dbcontex判断缓存key，
            // 根据dbcontext的schema等字段来区分不同的key，来达到刷新缓存的目的
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, DbContextCacheKeyFacotry>();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
