using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.Logs
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class LoginLogSetting
    {
        public int Id { get; set; }

        /// <summary>
        /// 保存时间（天）
        /// </summary>
        public int SaveTime { get; set; }
    }
}
