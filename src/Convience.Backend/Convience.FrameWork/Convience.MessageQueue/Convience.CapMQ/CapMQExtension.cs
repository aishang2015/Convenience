using Convience.InMemoryCap;
using Convience.SqlServerCap;
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

                case CapDataBaseType.SqlServer:
                    switch (mqType)
                    {
                        case CapMessageQueryType.RabbitMQ:
                            services.AddSqlServerRabbitMQCap<TDbContext>(dbConnectionString, mqConnectionString);
                            break;
                        case CapMessageQueryType.Kafka:
                            services.AddSqlServerKafkaCap<TDbContext>(dbConnectionString, mqConnectionString);
                            break;
                    }
                    break;

                case CapDataBaseType.InMemory:
                    switch (mqType)
                    {
                        case CapMessageQueryType.RabbitMQ:
                            services.AddInmemoryRabbitMQCap(mqConnectionString);
                            break;
                        case CapMessageQueryType.Kafka:
                            services.AddInmemoryKafkaCap(mqConnectionString);
                            break;
                    }
                    break;
            }

            return services;

        }
    }
}
