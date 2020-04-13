using Convience.Filestorage.Abstraction;

using Microsoft.Extensions.DependencyInjection;

namespace Convience.Filestorage.Filesystem
{
    public static class FileSystemStoreExtension
    {
        public static IServiceCollection AddFileSystemStore(this IServiceCollection services, string fileRootPath)
        {
            services.AddScoped<IFileStore>(provider => new FileSystemStore(fileRootPath));
            services.AddScoped<IFileStoreEntry, FileSystemStoreEntry>();
            return services;
        }
    }
}
