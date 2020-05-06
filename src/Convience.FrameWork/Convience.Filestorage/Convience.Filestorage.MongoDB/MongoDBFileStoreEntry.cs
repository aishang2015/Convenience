using Convience.Filestorage.Abstraction;

using MongoDB.Bson;
using MongoDB.Driver.GridFS;

using System;

namespace Convience.Filestorage.MongoDB
{
    public class MongoDBFileStoreEntry : IFileStoreEntry
    {
        public MongoDBFileStoreEntry() { }

        public MongoDBFileStoreEntry(GridFSFileInfo gridFSFileInfo, string path)
        {
            FileStoreId = gridFSFileInfo.Id.ToString();
            Name = gridFSFileInfo.Filename;
            Path = path;
            DirectoryPath = string.IsNullOrEmpty(Path.Substring(0, Path.Length - Name.Length).TrimEnd('/')) ?
                "/" : Path.Substring(0, Path.Length - Name.Length).TrimEnd('/');
            Length = gridFSFileInfo.Length;
            LastModifiedUtc = gridFSFileInfo.UploadDateTime;
            IsDirectory = false;
        }

        public ObjectId _id { get; set; }

        public string FileStoreId { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public string DirectoryPath { get; set; }

        public long Length { get; set; }

        public DateTime LastModifiedUtc { get; set; }

        public bool IsDirectory { get; set; }
    }
}
