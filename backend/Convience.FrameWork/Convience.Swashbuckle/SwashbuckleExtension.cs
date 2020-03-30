using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Convience.Swashbuckle
{
    public static class SwashbuckleExtension
    {
        public static IServiceCollection AddSwashbuckle(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend", Version = "v1" });
            });

            return services;
        }

        public static IApplicationBuilder UseSwashbuckle(this IApplicationBuilder app, string endpoint)
        {
            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend");
            });
            return app;
        }
    }
}
