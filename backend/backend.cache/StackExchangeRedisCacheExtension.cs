using Microsoft.Extensions.DependencyInjection;

namespace backend.cache
{
    public static class StackExchangeRedisCacheExtension
    {
        public static IServiceCollection AddCustomRedisCache(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "";
                options.InstanceName = "";
            });
            return services;
        }
    }
}
