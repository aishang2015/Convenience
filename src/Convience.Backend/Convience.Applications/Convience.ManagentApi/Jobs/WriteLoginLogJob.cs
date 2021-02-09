
using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;
using Convience.ManagentApi.Infrastructure.Logs.LoginLog;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Jobs
{
    public class WriteLoginLogJob
    {

        private readonly IRepository<LoginLogDetail> _loginLogDetail;

        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;

        // 每次最大处理量
        private const int BatchLimitCount = 1000;

        public WriteLoginLogJob(
            IRepository<LoginLogDetail> logDetailRrepository,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork)
        {
            _loginLogDetail = logDetailRrepository;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
        }

        public async Task Run()
        {
            int runCount = 0;
            while (!LoginLogQueue.Queue.IsEmpty && runCount++ < BatchLimitCount)
            {
                LoginLogQueue.Queue.TryDequeue(out LoginLogMessage message);

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
                }
            }
            await _systemIdentityDbUnitOfWork.SaveAsync();
        }
    }
}
