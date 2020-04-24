namespace Convience.Model.Models.ContentManage
{
    public class FileViewModel
    {
        public string FileName { get; set; }

        public string Directory { get; set; }

        public string Path => Directory != null ? Directory.Trim('/') : string.Empty + '/' + FileName;

        public bool IsDirectory => string.IsNullOrWhiteSpace(FileName);
    }
}
