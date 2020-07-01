using Convience.Filestorage.Abstraction;
using Convience.MongoDB;

using Microsoft.Extensions.Logging;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Filestorage.MongoDB
{
    public class MongoDBFileStore : IFileStore
    {
        private readonly MongoRepository _mongoRepository;

        private readonly ILogger<MongoDBFileStore> _logger;

        public MongoDBFileStore(MongoRepository mongoRepository,
            ILogger<MongoDBFileStore> logger)
        {
            _mongoRepository = mongoRepository;
            _logger = logger;
        }

        public async Task CopyFileAsync(string srcPath, string dstPath)
        {
            var fileEntity = await GetFileEntityByPath(srcPath);
            try
            {
                // 取得流=>取得新文件名=>保存新文件=>取得新文件信息=>保存新文件信息
                var stream = await _mongoRepository.GetFileStreamAsync(fileEntity._id);
                var newFileName = dstPath.Split('/').Last();
                var newid = await _mongoRepository.UploadFileAsync(newFileName, stream);
                var fileinfo = await _mongoRepository.GetFileByIdAsync(newid);
                await _mongoRepository.AddAsync(new MongoDBFileStoreEntry(fileinfo, dstPath));
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }
        }

        public async Task<string> CreateFileFromStreamAsync(string path, Stream inputStream, bool overwrite = false)
        {
            var fileName = path.Split('/').Last();
            try
            {
                // 保存文件=>取得文件信息=>保存文件信息
                var fileStoreId = await _mongoRepository.UploadFileAsync(fileName, inputStream);
                var fileinfo = await _mongoRepository.GetFileByIdAsync(fileStoreId);
                await _mongoRepository.AddAsync(new MongoDBFileStoreEntry(fileinfo, path));
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return null;
            }
            return path;
        }

        public async Task<IEnumerable<IFileStoreEntry>> GetDirectoryContentAsync(string path = "", bool includeSubDirectories = false)
        {
            var filter = Builders<MongoDBFileStoreEntry>.Filter.Where(e => e.DirectoryPath == path);
            var entityList = await _mongoRepository.GetAsync(filter);
            return entityList;
        }

        public async Task<IFileStoreEntry> GetDirectoryInfoAsync(string path)
        {
            var fileEntity = await GetFileEntityByPath(path);
            return fileEntity;
        }

        public async Task<IFileStoreEntry> GetFileInfoAsync(string path)
        {
            var fileEntity = await GetFileEntityByPath(path);
            return fileEntity;
        }

        public async Task<Stream> GetFileStreamAsync(string path)
        {
            var fileEntity = await GetFileEntityByPath(path);
            try
            {
                var stream = await _mongoRepository.GetFileStreamAsync(fileEntity.FileStoreId);
                return stream;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return null;
            }
        }

        public async Task<Stream> GetFileStreamAsync(IFileStoreEntry fileStoreEntry)
        {
            var fileEntity = await GetFileEntityByPath(fileStoreEntry.Path);
            try
            {
                var stream = await _mongoRepository.GetFileStreamAsync(fileEntity._id);
                return stream;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return null;
            }
        }

        public async Task MoveFileAsync(string oldPath, string newPath)
        {
            var fileEntity = await GetFileEntityByPath(oldPath);
            try
            {
                // 取得流=>取得新文件名=>保存新文件=>删除旧文件=>取得新文件信息=>更新文件信息
                var stream = await _mongoRepository.GetFileStreamAsync(fileEntity._id);
                var newFileName = newPath.Split('/').Last();
                var newid = await _mongoRepository.UploadFileAsync(newFileName, stream);
                await _mongoRepository.DeleteFileAsync(fileEntity.FileStoreId);

                var fileinfo = await _mongoRepository.GetFileByIdAsync(newid);
                fileEntity.Path = newPath;
                fileEntity.Name = fileinfo.Filename;
                fileEntity.DirectoryPath = newPath.Substring(0, newPath.Length - fileinfo.Filename.Length)
                    .TrimEnd('/');
                fileEntity.FileStoreId = fileinfo.Id.ToString();
                fileEntity.LastModifiedUtc = fileinfo.UploadDateTime;

                await _mongoRepository.UpdateAsync(fileEntity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }
        }

        public async Task<bool> TryCreateDirectoryAsync(string path)
        {
            var fileEntity = await GetFileEntityByPath(path);
            if (fileEntity == null)
            {
                var directoryPathArray = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var newArray = directoryPathArray.Take(directoryPathArray.Length - 1);
                var fileinfo = new MongoDBFileStoreEntry
                {
                    Path = path.TrimEnd('/'),
                    DirectoryPath = '/' + string.Join('/', newArray).TrimEnd('/'),
                    Name = directoryPathArray.Last(),
                    LastModifiedUtc = DateTime.Now,
                    IsDirectory = true
                };
                await _mongoRepository.AddAsync(fileinfo);
            }
            return true;
        }

        public async Task<bool> TryDeleteDirectoryAsync(string path)
        {
            path = path.TrimEnd('/');
            var filter = Builders<MongoDBFileStoreEntry>.Filter.Where(e => e.Path.StartsWith(path));
            var fileEntityList = await _mongoRepository.GetAsync(filter);
            try
            {
                foreach (var fileEntity in fileEntityList)
                {
                    if (!fileEntity.IsDirectory)
                    {
                        await _mongoRepository.DeleteFileAsync(fileEntity.FileStoreId);
                    }
                }
                await _mongoRepository.DeleteAsync(filter);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return false;
            }

        }

        public async Task<bool> TryDeleteFileAsync(string path)
        {
            var fileEntity = await GetFileEntityByPath(path);
            try
            {
                await _mongoRepository.DeleteFileAsync(fileEntity.FileStoreId);
                await _mongoRepository.DeleteAsync<MongoDBFileStoreEntry>(fileEntity._id.ToString());
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return false;
            }
        }


        private async Task<MongoDBFileStoreEntry> GetFileEntityByPath(string path)
        {
            path = path.TrimEnd('/');
            var filter = Builders<MongoDBFileStoreEntry>.Filter.Where(e => e.Path == path);
            var fileEntity = await _mongoRepository.GetAsync(filter);
            return fileEntity.FirstOrDefault();
        }
    }
}
