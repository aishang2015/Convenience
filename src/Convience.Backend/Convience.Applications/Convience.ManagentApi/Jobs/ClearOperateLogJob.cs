using AppService.Service;
using Convience.Entity.Data;
using Convience.Entity.Entity.OperateLog;
using Convience.EntityFrameWork.Repositories;
using System;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Jobs
{
    public class ClearOperateLogJob
    {
        private readonly IRepository<OperateLogDetail> _logDetaiRrepository;

        private readonly ICachingService<OperateLogSetting> _logSettingCaching;

        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;


        public ClearOperateLogJob(
            IRepository<OperateLogDetail> logDetaiRrepository,
            ICachingService<OperateLogSetting> logSettingCaching,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork)
        {
            _logDetaiRrepository = logDetaiRrepository;
            _logSettingCaching = logSettingCaching;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
        }

        public async Task Run()
        {
            // 清理过期的日志
            var settings = _logSettingCaching.GetCacheData();
            foreach (var setting in settings)
            {
                var expire = DateTime.Now.AddDays(-setting.SaveTime);
                await _logDetaiRrepository.RemoveAsync(d => d.OperateAt < expire && d.SettingId == setting.Id);
            }
            await _systemIdentityDbUnitOfWork.SaveAsync();
        }

    }
}
