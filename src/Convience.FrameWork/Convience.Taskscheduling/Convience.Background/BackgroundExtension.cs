using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Convience.Background
{
    public static class BackgroundExtension
    {
        public static IHostBuilder ConfigureHostedServices<T1>(this IHostBuilder hostBuilder)
            where T1 : class, IHostedService
        {
            hostBuilder.ConfigureServices(serivces =>
            {
                serivces.AddHostedService<T1>();
            });
            return hostBuilder;
        }

        public static IHostBuilder ConfigureHostedServices<TJob, TService>(this IHostBuilder hostBuilder)
            where TJob : AbstractBackgroundJob
            where TService : AbstractTimedBackgroundService
        {
            hostBuilder.ConfigureServices(serivces =>
            {
                serivces.AddHostedService<TService>();
                serivces.AddSingleton<TJob>();
            });
            return hostBuilder;
        }
    }
}
