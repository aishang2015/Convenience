using Convience.Filestorage.Abstraction;
using Convience.Model.Models.ContentManage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public class FileManageService : IFileManageService
    {

        private readonly IFileStore _fileStore;

        public FileManageService(IFileStore fileStore)
        {
            _fileStore = fileStore;
        }

        public async Task<bool> DeleteFileAsync(FileViewModel viewModel)
        {
            return await DeleteFileOrDirectory(viewModel);
        }

        public async Task<bool> DeleteDirectoryAsync(FileViewModel viewModel)
        {
            return await DeleteFileOrDirectory(viewModel);
        }

        public async Task<string> MakeDirectoryAsync(string directory)
        {
            var info = await _fileStore.GetFileInfoAsync(directory);
            if (info != null)
            {
                return "目录已存在！";
            }
            var isSuccess = await _fileStore.TryCreateDirectoryAsync(directory);
            return isSuccess ? string.Empty : "目录创建失败！";
        }

        public async Task<Stream> DownloadAsync(FileViewModel viewModel)
        {
            return await _fileStore.GetFileStreamAsync(viewModel.Path);
        }

        public async Task<IEnumerable<FileResult>> GetContentsAsync(FileQuery query)
        {
            var contents = await _fileStore.GetDirectoryContentAsync(query.Directory);
            var result = from content in contents
                         orderby content.Path
                         select new FileResult
                         {
                             FileName = content.Name,
                             Directory = content.DirectoryPath,
                             IsDirectory = content.IsDirectory,
                             CreateTime = content.LastModifiedUtc,
                             Size = content.Length
                         };

            var skip = query.Size * (query.Page - 1);
            return result.Skip(skip).Take(query.Size);
        }

        public async Task<string> UploadAsync(FileUploadModel viewModel)
        {
            foreach (var file in viewModel.Files)
            {
                var path = viewModel.CurrentDirectory?.TrimEnd('/') + '/' + file.FileName;
                var info = await _fileStore.GetFileInfoAsync(path);
                if (info != null)
                {
                    return "文件名冲突！";
                }
                var stream = file.OpenReadStream();
                var result = await _fileStore.CreateFileFromStreamAsync(path, stream);
                if (string.IsNullOrEmpty(result))
                {
                    return "文件上传失败！";
                }
            }
            return string.Empty;
        }

        private async Task<bool> DeleteFileOrDirectory(FileViewModel viewModel)
        {
            if (viewModel.IsDirectory)
            {
                return await _fileStore.TryDeleteDirectoryAsync(viewModel.Path);
            }
            else
            {
                return await _fileStore.TryDeleteFileAsync(viewModel.Path);
            }
        }
    }
}
