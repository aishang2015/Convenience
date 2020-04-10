
using Convience.Util.helpers;

using Microsoft.Extensions.DependencyInjection;

namespace Convience.Service
{
    public static class ServiceExtension
    {
        // 把所有Iservice和实现注入容器
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var pairs = ReflectionHelper.GetInterfaceAndImplementByName("Service");
            pairs.ForEach(pair => services.AddScoped(pair.Item1, pair.Item2));
            return services;
        }
    }
}
