using AppService.Service;
using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.BackgroudTask
{
    public class ClearLoginLogBackgroundService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public ClearLoginLogBackgroundService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var clearLoginLogService = scope.ServiceProvider
                        .GetRequiredService<IClearLoginLogService>();
                await clearLoginLogService.DoWork();
            }
        }
    }

    internal interface IClearLoginLogService
    {
        Task DoWork();
    }

    internal class ClearLoginLogService : IClearLoginLogService
    {
        private readonly IRepository<LoginLogDetail> _loginLogDetail;

        private readonly ICachingService<LoginLogSetting> _loginSetting;

        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;

        public ClearLoginLogService(
            IRepository<LoginLogDetail> loginLogDetail,
            ICachingService<LoginLogSetting> loginSetting,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork)
        {
            _loginLogDetail = loginLogDetail;
            _loginSetting = loginSetting;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
        }

        public async Task DoWork()
        {
            while (true)
            {
                // 清理过期的日志
                var setting = _loginSetting.GetCacheData(60 * 60 * 24).FirstOrDefault();
                if (setting != null)
                {
                    var expire = DateTime.Now.AddDays(-setting.SaveTime);
                    await _loginLogDetail.RemoveAsync(d => d.OperateAt < expire);
                }
                await _systemIdentityDbUnitOfWork.SaveAsync();

                // 一小时清理一次
                await Task.Delay(TimeSpan.FromHours(1));
            }
        }
    }
}
