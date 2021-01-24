using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

using System;

namespace Convience.Quartz
{
    /// <summary>
    /// 任务计划service扩展
    /// </summary>
    public static class QuartzExtension
    {
        /// <summary>
        /// 添加所有必须的services
        /// </summary>
        public static IServiceCollection AddQuartz(this IServiceCollection services, params (Type, string)[] jobs)
        {
            //添加Quartz服务
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // 添加jobRunner,将请求变为Scoped
            services.AddSingleton<QuartzJobRunner>();

            foreach (var job in jobs)
            {
                services.AddScoped(job.Item1);
                services.AddSingleton(new JobSchedule(jobType: job.Item1, cronExpression: job.Item2));
            }

            // 添加host服务
            services.AddHostedService<QuartzHostedService>();
            return services;
        }
    }
}
