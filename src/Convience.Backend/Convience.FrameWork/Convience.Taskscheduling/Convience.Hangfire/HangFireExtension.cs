using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;

using HangfireBasicAuthenticationFilter;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace Convience.Hangfire
{
    public static class HangFireExtension
    {
        public static IServiceCollection AddHF(this IServiceCollection services,
            HangFireDataBaseType hangFireDataBaseType,
            string connectionString)
        {
            switch (hangFireDataBaseType)
            {
                case HangFireDataBaseType.PostgreSQL:
                    services.AddPostgreSQLHangFire(connectionString);
                    break;
                case HangFireDataBaseType.SqlServer:
                    services.AddSqlServerHangFire(connectionString);
                    break;
                case HangFireDataBaseType.MySQL:
                    services.AddMySQLHangFire(connectionString);
                    break;
                case HangFireDataBaseType.InMemory:
                    services.AddInMemoryHangFire();
                    break;
            }
            return services;
        }

        public static IServiceCollection AddSqlServerHangFire(this IServiceCollection services,
            string connectionString)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(3),
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true,
                    PrepareSchemaIfNecessary = true,
                    SchemaName = "Hangfire"
                }));

            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddPostgreSQLHangFire(this IServiceCollection services,
            string connectionString)
        {
            services.AddHangfire(configuration =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                     .UseSimpleAssemblyNameTypeSerializer()
                     .UseRecommendedSerializerSettings()
                     .UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
                     {
                         QueuePollInterval = TimeSpan.FromSeconds(3),                   // 作业队列轮询间隔
                         PrepareSchemaIfNecessary = true,                               // 自动创建表
                         SchemaName = "Hangfire"                                        // Schema名
                     }).WithJobExpirationTimeout(TimeSpan.FromHours(1000));             // 作业过期时间，过期任务会被从数据库清理。此值不能小于1小时，否则会引起异常
            }).AddHangfireServer(option =>
            {
                option.SchedulePollingInterval = TimeSpan.FromSeconds(1);
            });

            return services;
        }

        public static IServiceCollection AddMySQLHangFire(this IServiceCollection services,
            string connectionString)
        {
            services.AddHangfire(configruation =>
            {
                configruation.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,   // 事务隔离级别
                        QueuePollInterval = TimeSpan.FromSeconds(3),                                    // 作业队列轮询间隔
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),                             // 作业过期检查间隔(管理过期记录)
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),                            // 计数器统计间隔
                        PrepareSchemaIfNecessary = true,                                                // 自动创建表
                        DashboardJobListLimit = 50000,                                                  // 仪表盘显示作业限制
                        TransactionTimeout = TimeSpan.FromMinutes(1),                                   // 事务超时时间
                        TablesPrefix = "T_Hangfire",                                                    // hangfire表名前缀
                        InvisibilityTimeout = TimeSpan.FromDays(1)                                      // 弃用属性，设定线程重开间隔
                    })).WithJobExpirationTimeout(TimeSpan.FromHours(24 * 7));         // 作业过期时间，过期任务会被从数据库清理。此值不能小于1小时，否则会引起异常

            }).AddHangfireServer(option =>
            {
                option.SchedulePollingInterval = TimeSpan.FromSeconds(1);
            });

            return services;
        }

        public static IServiceCollection AddInMemoryHangFire(this IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage());

            services.AddHangfireServer();

            return services;
        }

        public static IApplicationBuilder UseHFAnonymousDashBoard(this IApplicationBuilder app,
            string path = "/hangfire", bool readOnly = true)
        {
            app.UseHangfireDashboard(path, new DashboardOptions()
            {
                Authorization = new[] { new AllowAllAuthorizationFilter() },
                IsReadOnlyFunc = context => readOnly
            });

            //// 创建一个新作业
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));

            //// 延迟任务执行(Delayed jobs)
            //BackgroundJob.Schedule(() => Console.WriteLine("Delayed"), TimeSpan.FromSeconds(5));

            //// 定时任务执行(Recurring jobs)
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Hangfire is running Minutely."), Cron.Minutely());

            //// 延续性任务执行(Continuations)
            //var id = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, "));
            //BackgroundJob.ContinueJobWith(id, () => Console.WriteLine("world!"));


            return app;
        }


        public static IApplicationBuilder UseHFAuthorizeDashBoard(this IApplicationBuilder app,
            string path = "/hangfire", string userName = "admin", string password = "admin")
        {
            app.UseHangfireDashboard(path, new DashboardOptions
            {
                Authorization = new[] {
                    new HangfireCustomBasicAuthenticationFilter{
                        User = userName,
                        Pass =  password
                    }
                }
            });
            return app;
        }
    }
}
