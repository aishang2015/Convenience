using System;

namespace backend.filestorage.abstraction
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
