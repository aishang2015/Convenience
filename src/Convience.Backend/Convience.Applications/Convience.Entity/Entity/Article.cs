using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System;
using System.ComponentModel;

namespace Convience.Entity.Entity
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class Article
    {
        public int Id { get; set; }

        [Description("标题")]
        public string Title { get; set; }

        [Description("副标题")]
        public string SubTitle { get; set; }

        [Description("栏目ID")]
        public int? ColumnId { get; set; }

        [Description("出处")]
        public string Source { get; set; }

        [Description("排序")]
        public int Sort { get; set; }

        [Description("标签")]
        public string Tags { get; set; }

        [Description("内容")]
        public string Content { get; set; }

        [Description("更新时间")]
        public DateTime UpdateTime { get; set; }

        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

    }
}
