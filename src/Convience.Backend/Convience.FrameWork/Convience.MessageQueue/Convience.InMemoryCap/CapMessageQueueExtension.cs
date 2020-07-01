using Microsoft.Extensions.DependencyInjection;

namespace Convience.InMemoryCap
{
    public static class CapMessageQueueExtension
    {
        public static IServiceCollection AddInmemoryRabbitMQCap(this IServiceCollection services,
            string mqConnectionString)
        {
            services.AddCap(o =>
            {
                o.UseInMemoryStorage();
                o.UseRabbitMQ(mqConnectionString);
                o.UseDashboard();
            });
            return services;
        }

        public static IServiceCollection AddInmemoryKafkaCap(this IServiceCollection services,
            string mqConnectionString)
        {
            services.AddCap(o =>
            {
                o.UseInMemoryStorage();
                o.UseKafka(mqConnectionString);
                o.UseDashboard();
            });
            return services;
        }
    }
}
