using Convience.Automapper;
using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;
using Convience.EntityFrameWork.Repositories;
using Convience.Fluentvalidation;
using Convience.Hangfire;
using Convience.Jwtauthentication;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Repository;
using Convience.Service;
using Convience.Swashbuckle;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Convience.ManagentApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(services);

            services.AddApplicationDbContext(Configuration)
                .AddJwtBearer(Configuration)
                .AddPermissionAuthorization()
                .AddCorsPolicy()
                .AddSwashbuckle()
                .AddServices()
                .AddAutoMapper()
                .AddPostgreSQLHangFire(Configuration.GetConnectionString("PostgreSQL"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwashbuckle("backend");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseHFDashBoard();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class CustomExtension
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSQL");
            services.AddCustomDbContext<SystemIdentityDbContext, SystemUser, SystemRole, int>
                (connectionString, DataBaseType.PostgreSQL);

            services.AddRepositories<SystemIdentityDbContext>();

            services.AddRepositories();

            return services;
        }

        public static IServiceCollection AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration.GetSection("JwtOption"));
            services.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed(o => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            return services;
        }
    }
}
