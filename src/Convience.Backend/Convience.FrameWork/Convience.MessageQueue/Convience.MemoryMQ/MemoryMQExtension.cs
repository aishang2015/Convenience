using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Convience.MemoryMQ
{
    public static class MemoryMQExtension
    {

        public static IServiceCollection AddMemeoryMQ<T>(this IServiceCollection services, string name)
            where T : Good
        {
            if (MemeoryMQProvider.QueueDic.ContainsKey(name))
            {
                throw new Exception("queue key already exist！");
            }
            MemeoryMQProvider.QueueDic[name] = new ConcurrentQueue<T>();
            services.AddSingleton<MemeoryMQProvider>();
            return services;
        }
    }
}
