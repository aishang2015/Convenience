using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;

namespace Convience.Model.Models.ContentManage
{
    public class FileQueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string Directory { get; set; }
    }

    public class FileResultModel
    {
        public string FileName { get; set; }

        public string Directory { get; set; }

        public long Size { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDirectory { get; set; }
    }

    public class FileUploadViewModel
    {
        public string CurrentDirectory { get; set; }

        // 上传文件
        public IEnumerable<IFormFile> Files { get; set; }
    }

    public class FileViewModel
    {
        public string FileName { get; set; }

        public string Directory { get; set; }

        public string Path => (Directory != null ? Directory.TrimEnd('/') : string.Empty) + '/' + FileName;

        public bool IsDirectory => string.IsNullOrWhiteSpace(FileName);
    }
}
