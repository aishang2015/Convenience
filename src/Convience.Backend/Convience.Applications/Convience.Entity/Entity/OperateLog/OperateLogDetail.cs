using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;
using System;

namespace Convience.Entity.Entity.OperateLog
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class OperateLogDetail
    {
        public int Id { get; set; }


        public int SettingId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int OperateBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateAt { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 更新字段，关键数据变化可以切入逻辑中记录这三个字段
        /// </summary>
        public string UpdateField { get; set; }

        /// <summary>
        /// 旧值，关键数据变化可以切入逻辑中记录这三个字段
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// 新值，关键数据变化可以切入逻辑中记录这三个字段
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// 简单记录内容，报文从拦截器中获取
        /// </summary>
        public string Content { get; set; }
    }
}
