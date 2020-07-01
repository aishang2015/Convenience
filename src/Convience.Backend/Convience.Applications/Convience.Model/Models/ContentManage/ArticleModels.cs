using System;

namespace Convience.Model.Models.ContentManage
{

    public class ArticleQueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string Title { get; set; }

        public string Tag { get; set; }

        public int? ColumnId { get; set; }

    }

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

    public class ArticleResultModel : ArticleViewModel
    {
        public string ColumnName { get; set; }
    }
}
