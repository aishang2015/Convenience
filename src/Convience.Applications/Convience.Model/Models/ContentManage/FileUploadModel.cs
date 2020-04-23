using Microsoft.AspNetCore.Http;

namespace Convience.Model.Models.ContentManage
{
    public class FileUploadModel
    {
        public string CurrentDirectory { get; set; }

        // 上传文件
        public IFormFile File { get; set; }
    }
}
