using Microsoft.Extensions.Hosting;

using Quartz;
using Quartz.Spi;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.Quartz
{
    /// <summary>
    /// quartz的host服务
    /// </summary>
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<JobSchedule> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobSchedules = jobSchedules;
        }

        /// <summary>
        /// 调度器
        /// </summary>
        public IScheduler Scheduler { get; set; }

        /// <summary>
        /// 开始执行，随host启动
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            foreach (var jobSchedule in _jobSchedules)
            {
                // 创建job
                var job = CreateJob(jobSchedule);

                // 创建触发器
                var trigger = CreateTrigger(jobSchedule);

                // 添加调度
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
                jobSchedule.JobStatus = JobStatus.Scheduling;
            }

            // 执行调度器
            await Scheduler.Start(cancellationToken);
            foreach (var jobSchedule in _jobSchedules)
            {
                jobSchedule.JobStatus = JobStatus.Running;
            }
        }

        /// <summary>
        /// 停止执行，随host停止
        /// </summary>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
            foreach (var jobSchedule in _jobSchedules)
            {
                jobSchedule.JobStatus = JobStatus.Stopped;
            }
        }

        /// <summary>
        /// 生成工作实例
        /// </summary>
        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        /// <summary>
        /// 生成触发器实例
        /// </summary>
        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }
    }
}
