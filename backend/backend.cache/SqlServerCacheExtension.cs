using Microsoft.Extensions.DependencyInjection;

using System;

namespace backend.cache
{
    public static class SqlServerCacheExtension
    {
        public static IServiceCollection AddCustomSqlServerCache(this IServiceCollection services,
            string connectionString, string schemaName, string tableName,
            int defaultSlidingExpiration = 20,
            int expiredItemsDeletionInterval = 30)
        {
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = connectionString;
                options.SchemaName = schemaName;
                options.TableName = tableName;
                options.DefaultSlidingExpiration = TimeSpan.FromMinutes(defaultSlidingExpiration);
                options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(expiredItemsDeletionInterval);
            });
            return services;
        }
    }
}
