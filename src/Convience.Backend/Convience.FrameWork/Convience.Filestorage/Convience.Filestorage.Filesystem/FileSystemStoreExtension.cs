using Convience.Filestorage.Abstraction;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.Filestorage.Filesystem
{
    public static class FileSystemStoreExtension
    {
        public static IServiceCollection AddFileSystemStore(this IServiceCollection services, IConfiguration configuration)
        {
            var optionConfig = configuration.GetSection("FileSystemConfig");
            services.Configure<FileSystemStoreOption>(optionConfig);
            services.AddScoped<IFileStore, FileSystemStore>();
            services.AddScoped<IFileStoreEntry, FileSystemStoreEntry>();
            return services;
        }
    }
}
