using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Convience.Model.Models.ContentManage
{
    public class FileUploadModel
    {
        public string CurrentDirectory { get; set; }

        // 上传文件
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
