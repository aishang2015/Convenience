using Convience.Filestorage.Abstraction;
using Convience.MongoDB;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.Filestorage.MongoDB
{
    public static class MongoDBFileStoreExtension
    {
        public static IServiceCollection AddMongoDBFileStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDb(configuration);
            services.AddScoped<IFileStore, MongoDBFileStore>();
            services.AddScoped<IFileStoreEntry, MongoDBFileStoreEntry>();
            return services;
        }
    }
}
