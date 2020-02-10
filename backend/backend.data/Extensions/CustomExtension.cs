using backend.data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace backend.data.Extensions
{
    public static class CustomExtension
    {
        public static IServiceCollection AddCustomDbContext<TDbContext, TUser, TRole, Tkey>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : IdentityDbContext<TUser, TRole, Tkey>
            where TUser : IdentityUser<Tkey>
            where TRole : IdentityRole<Tkey>
            where Tkey : IEquatable<Tkey>
        {
            services.AddDbContext<TDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            })
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();


            return services;
        }
    }
}
