
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.CapMQ
{
    public static class CapMessageQueueExtension
    {

        public static IServiceCollection AddPostgreRabbitMQCap<TDbContext>(this IServiceCollection services,
            string dbConnectionString, string mqConnectionString)
            where TDbContext : DbContext
        {
            services.AddCap(o =>
            {
                o.UseEntityFramework<TDbContext>();
                o.UsePostgreSql(dbConnectionString);
                o.UseRabbitMQ(mqConnectionString);
                o.UseDashboard();
            });
            return services;
        }

        public static IServiceCollection AddPostgreKafkaCap<TDbContext>(this IServiceCollection services,
            string dbConnectionString, string mqConnectionString)
            where TDbContext : DbContext
        {
            services.AddCap(o =>
            {
                o.UseEntityFramework<TDbContext>();
                o.UsePostgreSql(dbConnectionString);
                o.UseKafka(mqConnectionString);
                o.UseDashboard();
            });
            return services;
        }


    }
}
