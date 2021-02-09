using AppService.Service;

using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;
using Convience.ManagentApi.Infrastructure.Logs;

using Microsoft.AspNetCore.Http;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Jobs
{
    public class WriteOperateLogJob
    {
        private readonly IRepository<OperateLogDetail> _logDetaiRrepository;

        private readonly ICachingService<OperateLogSetting> _logSettingCaching;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;

        // 每次最大处理量
        private const int BatchLimitCount = 1000;

        public WriteOperateLogJob(
            IRepository<OperateLogDetail> logDetaiRrepository,
            ICachingService<OperateLogSetting> logSettingCaching,
            IHttpContextAccessor httpContextAccessor,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork)
        {
            _logDetaiRrepository = logDetaiRrepository;
            _logSettingCaching = logSettingCaching;
            _httpContextAccessor = httpContextAccessor;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
        }

        public async Task Run()
        {
            int runCount = 0;
            while (!OperateLogQueue.Queue.IsEmpty && runCount++ < BatchLimitCount)
            {
                OperateLogQueue.Queue.TryDequeue(out OperateLogMessage message);

                // 有消息将数据写入数据库
                if (message != null)
                {
                    var settings = _logSettingCaching.GetCacheData(60 * 60 * 24);
                    var setting = (from s in settings
                                   where s.Action == message.Action && s.Controller == message.Controller
                                   select s).FirstOrDefault();
                    if (setting != null && setting.IsRecord)
                    {
                        var detail = new OperateLogDetail
                        {
                            OperateAt = DateTime.Now,
                            SettingId = setting.Id,
                            OperatorAccount = message.Account,
                            OperatorName = message.Name,
                            Content = message.RequestContent,
                            ResultCode = message.HttpResultCode,
                            Uri = message.Uri
                        };
                        await _logDetaiRrepository.AddAsync(detail);
                    }
                }
            }
            await _systemIdentityDbUnitOfWork.SaveAsync();
        }

    }
}
