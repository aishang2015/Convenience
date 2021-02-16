using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System;

namespace Convience.Entity.Entity.Logs
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class LoginLogDetail
    {
        public int Id { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorAccount { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateAt { get; set; }

        /// <summary>
        /// 登录人ip
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 是否登录成功
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
