using Convience.Util.Helpers;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Convience.Util.Extension
{
    public static class MediatRExtension
    {
        /// <summary>
        /// 引入所有程序集中的mediatR
        /// </summary>
        public static IServiceCollection AddAllMeidatR(this IServiceCollection services)
        {
            services.AddMediatR(ReflectionHelper.AssemblyList.ToArray());
            return services;
        }
    }
}
