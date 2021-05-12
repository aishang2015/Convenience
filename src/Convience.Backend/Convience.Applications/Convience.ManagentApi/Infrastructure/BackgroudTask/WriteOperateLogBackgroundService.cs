using AppService.Service;
using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;
using Convience.ManagentApi.Infrastructure.Logs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.BackgroudTask
{
    public class WriteOperateLogBackgroundService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public WriteOperateLogBackgroundService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var writeOperateLogService = scope.ServiceProvider
                        .GetRequiredService<IWriteOperateLogService>();
                await writeOperateLogService.DoWork();
            }
        }
    }

    internal interface IWriteOperateLogService
    {
        Task DoWork();
    }

    internal class WriteOperateLogService : IWriteOperateLogService
    {
        private readonly IRepository<OperateLogDetail> _logDetaiRrepository;

        private readonly ICachingService<OperateLogSetting> _logSettingCaching;

        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;

        public WriteOperateLogService(
            IRepository<OperateLogDetail> logDetaiRrepository,
            ICachingService<OperateLogSetting> logSettingCaching,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork)
        {
            _logDetaiRrepository = logDetaiRrepository;
            _logSettingCaching = logSettingCaching;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
        }

        public async Task DoWork()
        {
            // GetConsumingEnumerable在程序启动时会阻塞主线程，加上了这个delay就可以了
            await Task.Delay(1);
            foreach (var message in OperateLogQueue.blockingCollection.GetConsumingEnumerable())
            {
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
                        await _systemIdentityDbUnitOfWork.SaveAsync();
                    }
                }
            }

        }
    }
}
