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
            services.AddHostedService<EventListenerBackgroundService>();
            services.AddSingleton<SimpleEventListener>();
            return services;
        }
    }
}
