using Microsoft.Extensions.DependencyInjection;

namespace backend.cache
{
    public static class SqlServerCacheExtension
    {
        public static IServiceCollection AddCustomSqlServerCache(this IServiceCollection services)
        {
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = "";
                options.SchemaName = "";
                options.TableName = "";
            });
            return services;
        }
    }
}
