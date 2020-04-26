using System;

namespace Convience.Model.Models.ContentManage
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public int? ColumnId { get; set; }

        public string Source { get; set; }

        public int Sort { get; set; }

        public string Tags { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
