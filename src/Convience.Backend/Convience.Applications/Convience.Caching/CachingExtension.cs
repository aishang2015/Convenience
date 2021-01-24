
using AppService.Service;

using Convience.EntityFrameWork.Infrastructure;
using Convience.Util.Helpers;

using Microsoft.Extensions.DependencyInjection;

using System.Linq;

namespace Convience.Caching
{
    public static class CachingExtension
    {
        public static IServiceCollection AddCachingServices(this IServiceCollection services)
        {
            // 程序中不包含第三方的程序集
            var assemblyList = ReflectionHelper.AssemblyList;

            foreach (var types in assemblyList.Select(assembly => assembly.GetTypes().ToList()))
            {
                types.ForEach(type =>
                {
                    // 获取所有数据库相关实体
                    var attribute = type.GetCustomAttributes(typeof(EntityAttribute), false)
                        .FirstOrDefault();
                    if (attribute != null)
                    {
                        // 生成实体对应的接口类型和实现类型，加入容器
                        var interfaceSerivce = typeof(ICachingService<>).MakeGenericType(type);
                        var classSerivce = typeof(CachingService<>).MakeGenericType(type);
                        services.AddScoped(interfaceSerivce, classSerivce);
                    }
                });
            }

            return services;
        }
    }
}
