using Hangfire;

namespace Convience.ManagentApi.Jobs
{
    public class AllJobSetting
    {
        public static void SetJobs()
        {
            RecurringJob.AddOrUpdate<ResetUserAndMenuDataJob>("定时重置系统用户和菜单数据", j => j.Run(), Cron.Daily);

            RecurringJob.AddOrUpdate<WriteOperateLogJob>("定时写入操作日志", j => j.Run(), Cron.Minutely);
            RecurringJob.AddOrUpdate<ClearOperateLogJob>("定时清理操作日志", j => j.Run(), Cron.Daily);

            RecurringJob.AddOrUpdate<WriteLoginLogJob>("定时写入登录日志", j => j.Run(), Cron.Minutely);
            RecurringJob.AddOrUpdate<ClearLoginLogJob>("定时清理登录日志", j => j.Run(), Cron.Daily);
        }
    }
}
