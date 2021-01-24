using Microsoft.Extensions.DependencyInjection;

using Quartz;

using System;
using System.Threading.Tasks;

namespace Convience.Quartz
{
    /// <summary>
    /// scope范围的job
    /// </summary>
    public class QuartzJobRunner : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuartzJobRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 执行工作
        /// </summary>
        public async Task Execute(IJobExecutionContext context)
        {
            // 手动创建scope
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobType = context.JobDetail.JobType;
                var job = scope.ServiceProvider.GetRequiredService(jobType) as IJob;
                await job.Execute(context);
            }
        }
    }
}
