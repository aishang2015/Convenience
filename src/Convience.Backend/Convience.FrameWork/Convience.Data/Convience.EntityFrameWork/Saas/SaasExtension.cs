using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.EntityFrameWork.Saas
{
    public static class SaasExtension
    {
        /// <summary>
        /// 添加服务,通过该服务获取到当前用户的schema，在dbcontext构造时获取此schema，自动进行数据库
        /// schema切换
        /// </summary>
        public static IServiceCollection AddSchemaService<T>(this IServiceCollection services)
            where T : class, ISchemaService
        {
            services.AddScoped<ISchemaService, T>();
            return services;
        }

        /// <summary>
        /// 修改数据库连接串
        /// </summary>
        public static void ChangeDbContextConnection(this DbContext dbContext, string targetDbConnection)
        {
            dbContext.Database.GetDbConnection().ConnectionString = targetDbConnection;
        }
    }
}
