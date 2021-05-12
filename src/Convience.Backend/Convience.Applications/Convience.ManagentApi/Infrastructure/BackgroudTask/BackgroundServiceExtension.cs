using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.BackgroudTask
{
    public static class BackgroundServiceExtension
    {
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            // 日志相关
            services.AddScoped<IClearLoginLogService, ClearLoginLogService>();
            services.AddScoped<IClearOperateLogService, ClearOperateLogService>();
            services.AddScoped<IWriteLoginLogService, WriteLoginLogService>();
            services.AddScoped<IWriteOperateLogService, WriteOperateLogService>();
            services.AddHostedService<ClearLoginLogBackgroundService>();
            services.AddHostedService<ClearOperateLogBackgroundService>();
            services.AddHostedService<WriteLoginLogBackgroundService>();
            services.AddHostedService<WriteOperateLogBackgroundService>();

            // clr监控
            services.AddHostedService<EventListenerBackgroundService>();
            services.AddSingleton<SimpleEventListener>();
            return services;
        }
    }
}
