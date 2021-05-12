using AppService.Service;
using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.BackgroudTask
{
    public class ClearOperateLogBackgroundService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public ClearOperateLogBackgroundService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var clearOperateLogService = scope.ServiceProvider
                        .GetRequiredService<IClearOperateLogService>();
                await clearOperateLogService.DoWork();
            }
        }
    }

    internal interface IClearOperateLogService
    {
        Task DoWork();
    }

    internal class ClearOperateLogService : IClearOperateLogService
    {
        private readonly IRepository<OperateLogDetail> _logDetaiRepository;

        private readonly ICachingService<OperateLogSetting> _logSettingCaching;

        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;

        public ClearOperateLogService(
            IRepository<OperateLogDetail> logDetaiRepository,
            ICachingService<OperateLogSetting> logSettingCaching,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork)
        {
            _logDetaiRepository = logDetaiRepository;
            _logSettingCaching = logSettingCaching;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
        }

        public async Task DoWork()
        {
            while (true)
            {
                // 清理过期的日志
                var settings = _logSettingCaching.GetCacheData();
                foreach (var setting in settings)
                {
                    var expire = DateTime.Now.AddDays(-setting.SaveTime);
                    await _logDetaiRepository.RemoveAsync(d => d.OperateAt < expire && d.SettingId == setting.Id);
                }
                await _systemIdentityDbUnitOfWork.SaveAsync();

                // 一小时清理一次
                await Task.Delay(TimeSpan.FromHours(1));
            }
        }
    }
}
