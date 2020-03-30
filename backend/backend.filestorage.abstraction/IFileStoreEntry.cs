using System;

namespace Convience.Filestorage.Abstraction
{
    public interface IFileStoreEntry
    {
        string Path { get; }
        string Name { get; }
        string DirectoryPath { get; }
        long Length { get; }
        DateTime LastModifiedUtc { get; }
        bool IsDirectory { get; }
    }
}
