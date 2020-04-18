using DotNetCore.CAP;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace Convience.CapMQ
{
    public static class CapMessageQueueExtension
    {
        public enum CapMessageQueueType
        {
            RabbitMQ,
            //Kafka,
            //AzureServiceBus,
        }

        public static IServiceCollection AddPostgreCap<TDbContext>(this IServiceCollection services,
            string dbConnectionString,
            CapMessageQueueType messageQueueType,
            Action<RabbitMQOptions> configure)
            where TDbContext : DbContext
        {
            services.AddCap(o =>
            {
                o.UseEntityFramework<TDbContext>();

                o.UsePostgreSql(dbConnectionString);

                switch (messageQueueType)
                {
                    case CapMessageQueueType.RabbitMQ:
                        o.UseRabbitMQ(configure);
                        break;
                }

                o.UseDashboard();

            });
            return services;
        }


    }
}
