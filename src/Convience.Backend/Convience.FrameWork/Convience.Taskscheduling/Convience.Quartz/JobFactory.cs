using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Spi;

using System;

namespace Convience.Quartz
{
    /// <summary>
    /// 单例job工厂
    /// </summary>
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 在触发器触发时被调度器调用,生产出job实例
        /// </summary>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            // 通过在QuartzJobRunner内部execute创建Scope，然后再在scope内部
            // 执行scope的job
            return _serviceProvider.GetRequiredService<QuartzJobRunner>();
        }

        /// <summary>
        /// 允许作业工厂在必要时销毁/清理作业。
        /// </summary>
        public void ReturnJob(IJob job)
        {
        }
    }
}
