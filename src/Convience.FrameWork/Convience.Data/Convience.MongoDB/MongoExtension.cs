using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.MongoDB
{
    public static class MongoExtension
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOption>(configuration);
            services.AddSingleton<MongoClientContext>();
            services.AddSingleton<MongoRepository>();
            return services;
        }
    }
}
