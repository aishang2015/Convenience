using Convience.EntityFrameWork.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace Convience.EntityFrameWork.Infrastructure
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddCustomDbContext<TDbContext, TUser, TRole, Tkey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
            (this IServiceCollection services, string connectionString, DataBaseType dataBaseType)
                where TDbContext : IdentityDbContext<TUser, TRole, Tkey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
                where TUser : IdentityUser<Tkey>
                where TRole : IdentityRole<Tkey>
                where Tkey : IEquatable<Tkey>
                where TUserClaim : IdentityUserClaim<Tkey>
                where TUserRole : IdentityUserRole<Tkey>
                where TUserLogin : IdentityUserLogin<Tkey>
                where TRoleClaim : IdentityRoleClaim<Tkey>
                where TUserToken : IdentityUserToken<Tkey>
        {
            services.AddDbContext<TDbContext>(option =>
            {
                option = dataBaseType switch
                {
                    DataBaseType.SqlServer => option.UseSqlServer(connectionString),
                    DataBaseType.Sqlite => option.UseSqlite(connectionString),
                    DataBaseType.MySQL => option.UseMySql(connectionString, null),
                    DataBaseType.PostgreSQL => option.UseNpgsql(connectionString),
                    DataBaseType.Oracle => option.UseOracle(connectionString),
                    _ => option
                };

                // 启用日志参数
                option.EnableSensitiveDataLogging();
            }).AddIdentity<TUser, TRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredLength = 4;
            })
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IRepository<TUser>, BaseRepository<TUser, TDbContext>>();
            services.AddScoped<IRepository<TRole>, BaseRepository<TRole, TDbContext>>();
            services.AddScoped<IRepository<TUserClaim>, BaseRepository<TUserClaim, TDbContext>>();
            services.AddScoped<IRepository<TUserRole>, BaseRepository<TUserRole, TDbContext>>();
            services.AddScoped<IRepository<TUserLogin>, BaseRepository<TUserLogin, TDbContext>>();
            services.AddScoped<IRepository<TRoleClaim>, BaseRepository<TRoleClaim, TDbContext>>();
            services.AddScoped<IRepository<TUserToken>, BaseRepository<TUserToken, TDbContext>>();

            services.AddRepositories<TDbContext>();

            return services;
        }

        public static IServiceCollection AddCustomDbContext<TDbContext, TUser, TRole, Tkey>(this IServiceCollection services,
            string connectionString, DataBaseType dataBaseType)
            where TDbContext : IdentityDbContext<TUser, TRole, Tkey>
            where TUser : IdentityUser<Tkey>
            where TRole : IdentityRole<Tkey>
            where Tkey : IEquatable<Tkey>
        {
            services.AddDbContext<TDbContext>(option =>
            {
                option = dataBaseType switch
                {
                    DataBaseType.SqlServer => option.UseSqlServer(connectionString),
                    DataBaseType.Sqlite => option.UseSqlite(connectionString),
                    DataBaseType.MySQL => option.UseMySql(connectionString, null),
                    DataBaseType.PostgreSQL => option.UseNpgsql(connectionString),
                    DataBaseType.Oracle => option.UseOracle(connectionString),
                    _ => option
                };

                // 启用日志参数
                option.EnableSensitiveDataLogging();
            }).AddIdentity<TUser, TRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredLength = 4;
            })
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IRepository<TUser>, BaseRepository<TUser, TDbContext>>();
            services.AddScoped<IRepository<TRole>, BaseRepository<TRole, TDbContext>>();

            services.AddRepositories<TDbContext>();

            return services;
        }

        public static IServiceCollection AddCustomDbContext<TDbContext>(this IServiceCollection services,
            string connectionString, DataBaseType dataBaseType)
            where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(option =>
            {
                option = dataBaseType switch
                {
                    DataBaseType.SqlServer => option.UseSqlServer(connectionString),
                    DataBaseType.Sqlite => option.UseSqlite(connectionString),
                    DataBaseType.MySQL => option.UseMySql(connectionString, null),
                    DataBaseType.PostgreSQL => option.UseNpgsql(connectionString),
                    DataBaseType.Oracle => option.UseOracle(connectionString),
                    _ => option
                };

                // 启用日志参数
                option.EnableSensitiveDataLogging();
            });

            services.AddRepositories<TDbContext>();

            return services;
        }
    }
}