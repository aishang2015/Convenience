using Microsoft.Extensions.DependencyInjection;

using System;

namespace Convience.Cache
{
    public static class MemoryCacheExtension
    {
        public static IServiceCollection AddCustomMemoryCache(this IServiceCollection services,
            double compactionPercentage = 0.02d,
            double expirationScanFrequency = 5,
            long sizeLimit = 1024)
        {
            services.AddMemoryCache(options =>
            {
                // 压缩比
                options.CompactionPercentage = compactionPercentage;

                // 过期扫描间隔
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(expirationScanFrequency);

                // 大小
                options.SizeLimit = sizeLimit;
            });
            return services;
        }

    }
}
