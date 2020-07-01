using Microsoft.Extensions.DependencyInjection;

namespace Convience.Cache
{
    public static class StackExchangeRedisCacheExtension
    {
        public static IServiceCollection AddCustomRedisCache(this IServiceCollection services,
            string configuration,
            string instanceName)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration;
                options.InstanceName = instanceName;
            });
            return services;
        }
    }
}
