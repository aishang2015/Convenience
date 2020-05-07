using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.CapMQ
{
    public static class CapMQExtension
    {
        public static IServiceCollection AddCapMQ<TDbContext>(this IServiceCollection services,
            CapDataBaseType dbType,
            string dbConnectionString,
            CapMessageQueryType mqType,
            string mqConnectionString) where TDbContext : DbContext

        {
            switch (dbType)
            {
                case CapDataBaseType.PostgreSQL:
                    switch (mqType)
                    {
                        case CapMessageQueryType.RabbitMQ:
                            services.AddPostgreRabbitMQCap<TDbContext>(dbConnectionString, mqConnectionString);
                            break;
                        case CapMessageQueryType.Kafka:
                            services.AddPostgreKafkaCap<TDbContext>(dbConnectionString, mqConnectionString);
                            break;
                    }
                    break;
            }

            return services;

        }
    }
}
