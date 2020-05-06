using Convience.Model.Models.ContentManage;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public interface IFileManageService
    {
        public Task<string> UploadAsync(FileUploadModel viewModel);

        public Task<IEnumerable<FileResult>> GetContentsAsync(FileQuery query);

        public Task<bool> DeleteFileAsync(FileViewModel viewModel);

        public Task<Stream> DownloadAsync(FileViewModel viewModel);

        public Task<string> MakeDirectoryAsync(FileViewModel viewModel);

        public Task<bool> DeleteDirectoryAsync(FileViewModel viewModel);

    }
}
