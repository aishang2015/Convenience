using Microsoft.Extensions.DependencyInjection;

namespace Convience.HttpClients
{
    public static class HttpClientExtension
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddHttpClients<T>(this IServiceCollection services)
            where T : AbstractHttpClient
        {
            services.AddHttpClient<T>();
            return services;
        }
    }
}
