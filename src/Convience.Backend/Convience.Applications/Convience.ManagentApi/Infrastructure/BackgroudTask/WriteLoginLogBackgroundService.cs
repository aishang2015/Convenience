using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;
using Convience.ManagentApi.Infrastructure.Logs.LoginLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.BackgroudTask
{
    public class WriteLoginLogBackgroundService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public WriteLoginLogBackgroundService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var writeLoginLogService = scope.ServiceProvider
                        .GetRequiredService<IWriteLoginLogService>();
                await writeLoginLogService.DoWork();
            }
        }
    }


    internal interface IWriteLoginLogService
    {
        Task DoWork();
    }

    internal class WriteLoginLogService : IWriteLoginLogService
    {
        private readonly IRepository<LoginLogDetail> _loginLogDetail;

        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;

        public WriteLoginLogService(
            IRepository<LoginLogDetail> loginLogDetail,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork)
        {
            _loginLogDetail = loginLogDetail;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
        }

        public async Task DoWork()
        {
            // GetConsumingEnumerable在程序启动时会阻塞主线程，加上了这个delay就可以了
            await Task.Delay(1);
            foreach (var message in LoginLogQueue.blockingCollection.GetConsumingEnumerable())
            {
                // 有消息将数据写入数据库
                if (message != null)
                {
                    var detail = new LoginLogDetail
                    {
                        OperateAt = message.OperateAt,
                        OperatorAccount = message.OperatorAccount,
                        IpAddress = message.IpAddress,
                        IsSuccess = message.IsSuccess,
                    };
                    await _loginLogDetail.AddAsync(detail);
                    await _systemIdentityDbUnitOfWork.SaveAsync();
                }
            }


        }
    }
}
