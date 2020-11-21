using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.OperateLog
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
        /// 访问资源地址
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 资源方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 保存时间（单位天）
        /// </summary>
        public int SaveTime { get; set; }

        /// <summary>
        /// 是否记录此类记录
        /// </summary>
        public int IsRecord { get; set; }
    }
}
