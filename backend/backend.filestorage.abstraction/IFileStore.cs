using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Filestorage.Abstraction
{
    public interface IFileStore
    {
        Task<IFileStoreEntry> GetFileInfoAsync(string path);

        Task<IFileStoreEntry> GetDirectoryInfoAsync(string path);

        Task<IEnumerable<IFileStoreEntry>> GetDirectoryContentAsync(string path = null, bool includeSubDirectories = false);

        Task<bool> TryCreateDirectoryAsync(string path);

        Task<bool> TryDeleteFileAsync(string path);

        Task<bool> TryDeleteDirectoryAsync(string path);

        Task MoveFileAsync(string oldPath, string newPath);

        Task CopyFileAsync(string srcPath, string dstPath);

        Task<Stream> GetFileStreamAsync(string path);

        Task<Stream> GetFileStreamAsync(IFileStoreEntry fileStoreEntry);

        Task<string> CreateFileFromStreamAsync(string path, Stream inputStream, bool overwrite = false);
    }

    public static class IFileStoreExtensions
    {
        public static string Combine(this IFileStore fileStore, params string[] paths)
        {
            if (paths.Length == 0)
                return null;

            var normalizedParts = paths
                    .Select(x => fileStore.NormalizePath(x))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();

            var combined = string.Join("/", normalizedParts);

            if (paths[0]?.StartsWith('/') == true)
                combined = "/" + combined;

            return combined;
        }

        public static string NormalizePath(this IFileStore fileStore, string path)
        {
            if (path == null)
                return null;

            return path.Replace('\\', '/').Trim('/');
        }
    }
}
