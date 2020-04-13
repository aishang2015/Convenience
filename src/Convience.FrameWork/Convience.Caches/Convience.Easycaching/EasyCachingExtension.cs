using Convience.Easycaching;

using EasyCaching.Core.Configurations;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

namespace Convience.Easycaching
{
    public static class EasyCachingExtension
    {
        // 参数：类型，名称，地址，端口
        public static IServiceCollection AddEasyCaching(this IServiceCollection services,
            List<(CacheType, string, string, int)> configList)
        {
            services.AddEasyCaching(options =>
            {
                foreach (var config in configList)
                {
                    switch (config.Item1)
                    {
                        case CacheType.InMemory:
                            options.UseInMemory(config.Item2);
                            break;
                        case CacheType.Redis:
                            options.UseRedis(c =>
                            {
                                c.DBConfig.Endpoints.Add(new ServerEndPoint(config.Item3, config.Item4));
                            }, config.Item2);
                            break;
                    }
                }
            });
            return services;
        }
    }

    public enum CacheType
    {
        InMemory,
        Redis
    }
}
