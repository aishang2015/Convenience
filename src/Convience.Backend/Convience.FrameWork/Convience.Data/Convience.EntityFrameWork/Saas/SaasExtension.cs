using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.EntityFrameWork.Saas
{
    public static class SaasExtension
    {
        /// <summary>
        /// 添加服务
        /// </summary>
        public static IServiceCollection AddSchemaService(this IServiceCollection services)
        {
            services.AddScoped<ISchemaService, SchemaService>();
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
