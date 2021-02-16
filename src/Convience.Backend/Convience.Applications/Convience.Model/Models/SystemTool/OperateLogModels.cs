using Convience.Entity.Entity.Logs;

using System;

namespace Convience.Model.Models.SystemTool
{

    #region OperateLogSetting Models

    public class OperateLogDetailResultModel : OperateLogDetail
    {
        /// <summary>
        /// 模块名
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 子模块名
        /// </summary>
        public string SubModuleName { get; set; }

        /// <summary>
        /// 功能
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 保存时间（单位天）
        /// </summary>
        public int SaveTime { get; set; }
    }

    public class OperateLogDetailQueryModel : PageSortQueryModel
    {
        public string Module { get; set; }
        public string Submodule { get; set; }
        public string Operator { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }

    #endregion

    #region OperateLogSetting Models

    public class OperateLogSettingViewModel
    {
        public int Id { get; set; }

        public int SaveTime { get; set; }

        public bool IsRecord { get; set; }
    }

    public class OperateLogSettingResultModel : OperateLogSetting
    {
    }

    public class OperateLogSettingQueryModel : PageSortQueryModel
    {
        public string Module { get; set; }
        public string Submodule { get; set; }
    }

    #endregion
}
