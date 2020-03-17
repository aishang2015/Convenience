using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace backend.data.Infrastructure
{
    public class AppDbContextSeed
    {
        public static void InitialApplicationDataBase(AppIdentityDbContext dbContext,
            IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var user = new AppUser
            {
                UserName = $"admin",
            };
            userManager.CreateAsync(user, "admin").Wait();
        }
    }
}
