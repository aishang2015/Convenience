using Convience.Automapper;
using Convience.CapMQ;
using Convience.Easycaching;
using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;
using Convience.EntityFrameWork.Repositories;
using Convience.Filestorage.MongoDB;
using Convience.Fluentvalidation;
using Convience.Hangfire;
using Convience.Jwtauthentication;
using Convience.ManagentApi.Infrastructure;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Service;
using Convience.Swashbuckle;
using Convience.Util.Middlewares;
using Hangfire;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;

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
            var dbConnectionString = Configuration.GetConnectionString("PostgreSQL");
            var mqConnectionString = Configuration.GetConnectionString("RabbitMQ");
            var mdbConnectionConfig = Configuration.GetSection("MongoDb");

            services.AddControllers().AddNewtonsoftJson()
                .AddFluentValidation(services);

            services.AddApplicationDbContext(dbConnectionString)
                .AddJwtBearer(Configuration)
                .AddPermissionAuthorization()
                .AddCorsPolicy()
                .AddSwashbuckle()
                .AddServices()
                .AddAutoMapper()
                .AddPostgreHangFire(dbConnectionString)
                .AddPostgreCap(dbConnectionString, mqConnectionString)
                .AddMemoryCache()
                .AddMongoDBFileManage(mdbConnectionConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<CustomExceptionMiddleware>();
            }

            app.UseSwashbuckle("backend");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseHangfireDashBoard(serviceProvider);

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
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddCustomDbContext<SystemIdentityDbContext, SystemUser, SystemRole, int>
                (connectionString, DataBaseType.PostgreSQL);

            services.AddRepositories<SystemIdentityDbContext>();

            services.AddRepositories();

            return services;
        }

        public static IServiceCollection AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(null, configuration.GetSection("JwtOption"));
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

        public static IServiceCollection AddMemoryCache(this IServiceCollection services)
        {
            var configs = new List<(CacheType, string, string, int)>() {
                (CacheType.InMemory,"defaultMemoryCache",null,0)
            };
            services.AddEasyCaching(configs);
            return services;
        }

        public static IServiceCollection AddPostgreHangFire(this IServiceCollection services, string connectionString)
        {
            services.AddHF(HangFireDataBaseType.PostgreSQL, connectionString);
            return services;
        }

        public static IServiceCollection AddMongoDBFileManage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDBFileStore(configuration);
            return services;
        }

        public static IServiceCollection AddPostgreCap(this IServiceCollection services, string dbConnectionString, string mqConnectionString)
        {
            services.AddCapMQ<SystemIdentityDbContext>(CapDataBaseType.PostgreSQL, dbConnectionString,
                 CapMessageQueryType.RabbitMQ, mqConnectionString);
            return services;
        }


        public static IApplicationBuilder UseHangfireDashBoard(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            GlobalConfiguration.Configuration.UseActivator(new HangFireJobActivator(serviceProvider.GetService<IServiceScopeFactory>()));

            app.UseHFDashBoard();
            app.UseHFDashBoard("/taskManage");

            RecurringJob.AddOrUpdate<HangfireResetDataJob>("JobIOCA", j => j.Run(), Cron.Daily);
            return app;
        }
    }
}
