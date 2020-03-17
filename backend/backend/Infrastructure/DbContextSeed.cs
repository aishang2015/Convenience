using backend.api.Infrastruct;
using backend.entity.backend.api;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace backend.api.Infrastructure
{
    public class DbContextSeed
    {
        public static void InitialApplicationDataBase(SystemIdentityDbContext dbContext,
            IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<SystemUser>>();
            var user = new SystemUser
            {
                UserName = $"admin",
            };
            userManager.CreateAsync(user, "admin").Wait();
        }
    }
}
