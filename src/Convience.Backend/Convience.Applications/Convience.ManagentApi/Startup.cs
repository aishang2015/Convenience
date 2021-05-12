using Convience.Caching;
using Convience.Entity.Data;
using Convience.Entity.Entity.Identity;
using Convience.EntityFrameWork.Infrastructure;
using Convience.Filestorage.Filesystem;
using Convience.Filestorage.MongoDB;
using Convience.Fluentvalidation;
using Convience.Hangfire;
using Convience.Injection;
using Convience.JwtAuthentication;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.BackgroudTask;
using Convience.ManagentApi.Infrastructure.Hubs;
using Convience.SignalRs;
using Convience.Util.Extension;
using Convience.Util.Middlewares;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

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
            var jwtOption = Configuration.GetSection("JwtOption");

            services.AddControllers().AddControllersAsServices().AddNewtonsoftJson()
                .AddFluentValidation(services);

            services.AddApplicationDbContext(dbConnectionString)
                .AddJwtBearer(jwtOption)
                .AddPermissionAuthorization()
                .AddCorsPolicy()
                .AddSwashbuckle()
                .AddAutoMapper()
                .AddMemoryCache()
                .AddFileSystemStore(Configuration)
                .AddServices()
                .AddCachingServices()
                .AddResponseCompression()
                .AddAutowired()
                .AddBackgroundServices()
                .AddSignalR();
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

            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });

            app.UseSwashbuckle("backend");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TestHub>("/hubs");
                endpoints.MapHub<AppStateHub>("/appState");
                endpoints.MapControllers();
            });
        }
    }

    public static class CustomExtension
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddCustomDbContext<SystemIdentityDbContext, SystemUser, SystemRole, int, SystemUserClaim,
                SystemUserRole, SystemUserLogin, SystemRoleClaim, SystemUserToken>
                (connectionString, DataBaseType.PostgreSQL);

            services.AddScoped<SystemIdentityDbUnitOfWork>();

            return services;
        }

        public static IServiceCollection AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration, null, new SignalRJwtBearerEvents());
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

        public static IServiceCollection AddPostgreHangFire(this IServiceCollection services, string connectionString)
        {
            services.AddHF(HangFireDataBaseType.PostgreSQL, connectionString);
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScopedBatch(t => t.Name.EndsWith("Service") && t.IsInterface);
            return services;
        }

        public static IServiceCollection AddMongoDBFileManage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDBFileStore(configuration);
            return services;
        }


        public static IApplicationBuilder UseHangfireDashBoard(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseHFAuthorizeDashBoard("/taskManage");
            app.UseHFAnonymousDashBoard("/taskView");

            //RecurringJob.AddOrUpdate<WriteOperateLogJob>("定时写入操作日志", j => j.Run(), Cron.Minutely);
            //RecurringJob.AddOrUpdate<ClearOperateLogJob>("定时清理操作日志", j => j.Run(), Cron.Daily);

            //RecurringJob.AddOrUpdate<WriteLoginLogJob>("定时写入登录日志", j => j.Run(), Cron.Minutely);
            //RecurringJob.AddOrUpdate<ClearLoginLogJob>("定时清理登录日志", j => j.Run(), Cron.Daily);
            return app;
        }
    }
}
