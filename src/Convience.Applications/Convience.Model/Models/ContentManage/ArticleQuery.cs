namespace Convience.Model.Models.ContentManage
{
    public class ArticleQuery
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string Title { get; set; }

        public string Tag { get; set; }

        public int? ColumnId { get; set; }

    }
}
