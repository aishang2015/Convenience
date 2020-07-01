using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.WorkFlows
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlowFormControl
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // 控件类型
        public ControlTypeEnum ControlType { get; set; }

        #region 控件属性

        public string DomId { get; set; }

        // 控件位置
        public int Top { get; set; }

        // 控件位置
        public int Left { get; set; }

        // 宽度
        public int Width { get; set; }

        // 字体大小
        public int FontSize { get; set; }

        // 内容，针对label
        public string Content { get; set; }

        // 行数，针对textarea
        public int? Line { get; set; }

        // 选项，针对select
        public string Options { get; set; }

        #endregion

        #region 控件验证

        // 是否必须
        public bool? IsRequired { get; set; }

        // 正则验证
        public string Parttern { get; set; }

        // 最大输入长度
        public int? MaxLength { get; set; }

        #endregion

        #region

        public int WorkFlowId { get; set; }

        public WorkFlow WorkFlow { get; set; }

        #endregion

    }

    public enum ControlTypeEnum
    {
        Label = 1,
        TextBox = 2,
        Select = 3,
        Number = 4,
        Date = 5,
        DateTime = 6,
        TextArea = 7,
    }
}
