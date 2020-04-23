using System;

namespace Convience.Model.Models.ContentManage
{
    public class FileResult
    {
        public string FileName { get; set; }

        public string Directory { get; set; }

        public long Size { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDirectory { get; set; }
    }
}
