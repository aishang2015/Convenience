using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
