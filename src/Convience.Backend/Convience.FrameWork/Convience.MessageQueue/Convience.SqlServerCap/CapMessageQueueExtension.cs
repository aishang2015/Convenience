using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.SqlServerCap
{
    public static class CapMessageQueueExtension
    {
        public static IServiceCollection AddSqlServerRabbitMQCap<TDbContext>(this IServiceCollection services,
               string dbConnectionString, string mqConnectionString)
               where TDbContext : DbContext
        {
            services.AddCap(o =>
            {
                o.UseEntityFramework<TDbContext>();
                o.UseSqlServer(dbConnectionString);
                o.UseRabbitMQ(mqConnectionString);

                // 使用可视面板
                o.UseDashboard();

                // 重试次数
                o.FailedRetryCount = 5;

                // 清理成功的消息记录
                o.SucceedMessageExpiredAfter = 3600 * 24 * 2;
            });
            return services;
        }

        public static IServiceCollection AddSqlServerKafkaCap<TDbContext>(this IServiceCollection services,
            string dbConnectionString, string mqConnectionString)
            where TDbContext : DbContext
        {
            services.AddCap(o =>
            {
                o.UseEntityFramework<TDbContext>();
                o.UseSqlServer(dbConnectionString);
                o.UseKafka(mqConnectionString);

                // 使用可视面板
                o.UseDashboard();

                // 重试次数
                o.FailedRetryCount = 5;

                // 清理成功的消息记录
                o.SucceedMessageExpiredAfter = 3600 * 24 * 2;
            });
            return services;
        }
    }
}
