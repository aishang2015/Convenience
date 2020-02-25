using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

namespace backend.easycaching
{
    public static class EasyCachingExtension
    {
        public static void AddEasyCaching(this IServiceCollection services,
            IConfiguration configuration,
            List<(CacheType, string, string)> configList)
        {
            services.AddEasyCaching(options =>
            {
                foreach (var config in configList)
                {
                    switch (config.Item1)
                    {
                        case CacheType.InMemory:
                            options.UseInMemory(configuration, config.Item2, config.Item3);
                            break;
                        case CacheType.Redis:
                            options.UseRedis(configuration, config.Item2, config.Item3);
                            break;
                    }
                }
            });
        }
    }

    public enum CacheType
    {
        InMemory,
        Redis
    }
}
