using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace backend.data.Infrastructure
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddCustomDbContext<TDbContext, TUser, TRole, Tkey>(this IServiceCollection services,
            string connectionString, DataBaseType dataBaseType)
            where TDbContext : IdentityDbContext<TUser, TRole, Tkey>
            where TUser : IdentityUser<Tkey>
            where TRole : IdentityRole<Tkey>
            where Tkey : IEquatable<Tkey>
        {
            services.AddDbContext<TDbContext>(option =>
            {
                switch (dataBaseType)
                {
                    case DataBaseType.SqlServer:
                        option.UseSqlServer(connectionString);
                        break;
                    case DataBaseType.Sqlite:
                        option.UseSqlite(connectionString);
                        break;
                    case DataBaseType.MySQL:
                        option.UseMySql(connectionString);
                        break;
                    case DataBaseType.PostgreSQL:
                        option.UseNpgsql(connectionString);
                        break;
                }
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
            return services;
        }

        public static IServiceCollection AddCustomDbContext<TDbContext>(this IServiceCollection services,
            string connectionString, DataBaseType dataBaseType)
            where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(option =>
            {
                switch (dataBaseType)
                {
                    case DataBaseType.SqlServer:
                        option.UseSqlServer(connectionString);
                        break;
                    case DataBaseType.Sqlite:
                        option.UseSqlite(connectionString);
                        break;
                    case DataBaseType.MySQL:
                        option.UseMySql(connectionString);
                        break;
                    case DataBaseType.PostgreSQL:
                        option.UseNpgsql(connectionString);
                        break;
                }
            });
            return services;
        }
    }
}