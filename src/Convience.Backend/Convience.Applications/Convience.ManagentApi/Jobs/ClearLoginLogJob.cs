using AppService.Service;

using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Jobs
{
    public class ClearLoginLogJob
    {

        private readonly IRepository<LoginLogDetail> _loginLogDetail;

        private readonly ICachingService<LoginLogSetting> _loginSetting;

        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;


        public ClearLoginLogJob(
            IRepository<LoginLogDetail> logDetailRrepository,
            ICachingService<LoginLogSetting> loginSetting,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork)
        {
            _loginLogDetail = logDetailRrepository;
            _loginSetting = loginSetting;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
        }

        public async Task Run()
        {
            // 清理过期的日志
            var setting = _loginSetting.GetCacheData(60 * 60 * 24).FirstOrDefault();
            if (setting != null)
            {
                var expire = DateTime.Now.AddDays(-setting.SaveTime);
                await _loginLogDetail.RemoveAsync(d => d.OperateAt < expire);
            }
            await _systemIdentityDbUnitOfWork.SaveAsync();
        }
    }
}
