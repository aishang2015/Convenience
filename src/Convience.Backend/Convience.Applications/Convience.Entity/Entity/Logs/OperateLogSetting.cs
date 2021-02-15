using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.Logs
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class OperateLogSetting
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

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
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 行为
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 保存时间（单位天）
        /// </summary>
        public int SaveTime { get; set; }

        /// <summary>
        /// 是否记录此类记录
        /// </summary>
        public bool IsRecord { get; set; }
    }
}
