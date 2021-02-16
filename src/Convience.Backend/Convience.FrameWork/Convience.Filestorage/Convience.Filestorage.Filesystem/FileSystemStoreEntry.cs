using Convience.Filestorage.Abstraction;

using Microsoft.Extensions.FileProviders;

using System;

namespace Convience.Filestorage.Filesystem
{
    public class FileSystemStoreEntry : IFileStoreEntry
    {
        private readonly IFileInfo _fileInfo;
        private readonly string _path;

        public FileSystemStoreEntry() { }

        public FileSystemStoreEntry(string path, IFileInfo fileInfo)
        {
            _fileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string Path => _path;

        public string Name => _fileInfo.Name;

        public string DirectoryPath => _path.Substring(0, _path.Length - Name.Length).TrimEnd('/');

        public long Length => _fileInfo.Length;

        public DateTime LastModifiedUtc => _fileInfo.LastModified.UtcDateTime;

        public bool IsDirectory => _fileInfo.IsDirectory;
    }
}
