using Microsoft.Extensions.DependencyInjection;

namespace Convience.EntityFrameWork.Saas
{
    public static class SaasExtension
    {
        public static IServiceCollection AddSchemaService(this IServiceCollection services)
        {
            services.AddScoped<ISchemaService, SchemaService>();
            return services;
        }
    }
}
