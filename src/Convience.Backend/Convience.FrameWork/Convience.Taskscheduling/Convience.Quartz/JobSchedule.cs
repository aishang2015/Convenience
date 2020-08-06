using System;
using System.ComponentModel;

namespace Convience.Quartz
{
    /// <summary>
    /// Job调度中间对象
    /// </summary>
    public class JobSchedule
    {
        /// <summary>
        /// Job类型
        /// </summary>
        public Type JobType { get; private set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronExpression { get; private set; }

        /// <summary>
        /// Job状态
        /// </summary>
        public JobStatus JobStatus { get; set; } = JobStatus.Init;

        /// <summary>
        /// 构造函数
        /// </summary>
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }
    }

    /// <summary>
    /// Job运行状态
    /// </summary>
    public enum JobStatus : byte
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Description("初始化")]
        Init = 0,

        /// <summary>
        /// 运行中
        /// </summary>
        [Description("运行中")]
        Running = 1,

        /// <summary>
        /// 调度中
        /// </summary>
        [Description("调度中")]
        Scheduling = 2,

        /// <summary>
        /// 已停止
        /// </summary>
        [Description("已停止")]
        Stopped = 3,

    }
}
